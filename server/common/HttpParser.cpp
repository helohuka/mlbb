#include "config.h"
#include "HttpParser.h"

//////////////////////////////////////////////////////////////////////////

// = Static initialization.
const char *const
HTTP_Helper::months_[12]=
{
	"Jan", "Feb", "Mar", "Apr", "May", "Jun",
	"Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
};

char const *HTTP_Helper::alphabet_ = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

char * HTTP_Helper::date_string_ = 0;
ACE_SYNCH_MUTEX HTTP_Helper::mutex_;

ACE_SYNCH_MUTEX HTTP_Status_Code::lock_;
int HTTP_Status_Code::instance_ = 0;
const char *HTTP_Status_Code::Reason[HTTP_Status_Code::MAX_STATUS_CODE + 1];

time_t
HTTP_Helper::HTTP_mktime (const char *httpdate)
{
	char *buf;

	ACE_NEW_RETURN (buf, char[ACE_OS::strlen (httpdate) + 1], (time_t) -1);

	// Make spaces in the date be semi-colons so we can parse robustly
	// with sscanf.

	const char *ptr1 = httpdate;
	char *ptr2 = buf;

	do
	{
		if (*ptr1 == ' ')
			*ptr2++ = ';';
		else
			*ptr2++ = *ptr1;
	}
	while (*ptr1++ != '\0');

	// In HTTP/1.0, there are three versions of an HTTP_date.

	// rfc1123-date   = wkday "," SP dd month yyyy SP hh:mm:ss SP "GMT"
	// rfc850-date    = weekday "," SP dd-month-yy SP hh:mm:ss SP "GMT"
	// asctime-date   = wkday SP month dd SP hh:mm:ss SP yyyy

	// static const char rfc1123_date[] = "%3s,;%2d;%3s;%4d;%2d:%2d:%2d;GMT";
	// static const char rfc850_date[]  = "%s,;%2d-%3s-%2d;%2d:%2d:%2d;GMT";
	// static const char asctime_date[] = "%3s;%3s;%2d;%2d:%2d:%2d;%4d";

	// Should also support other versions (such as from NNTP and SMTP)
	// for robustness, but it should be clear how to extend this.

	struct tm tms;
	char month[4];
	char weekday[10];

	if (::sscanf(buf, "%3s,;%2d;%3s;%4d;%2d:%2d:%2d;GMT", // RFC-1123 date format
		weekday,
		&tms.tm_mday,
		month,
		&tms.tm_year,
		&tms.tm_hour,
		&tms.tm_min,
		&tms.tm_sec) == 7)
		;
	else if (::sscanf(buf, "%s,;%2d-%3s-%2d;%2d:%2d:%2d;GMT", // RFC-850 date format
		weekday,
		&tms.tm_mday, month, &tms.tm_year,
		&tms.tm_hour, &tms.tm_min, &tms.tm_sec) == 7)
	{
		weekday[3] = '\0';
	}
	else if (::sscanf(buf, "%3s;%3s;%2d;%2d:%2d:%2d;%4d", // ASCTIME date format.
		weekday,
		month, &tms.tm_mday,
		&tms.tm_hour, &tms.tm_min, &tms.tm_sec,
		&tms.tm_year) == 7)
	{
	}

	delete [] buf;

	tms.tm_year = HTTP_Helper::fixyear (tms.tm_year);
	tms.tm_mon = HTTP_Helper::HTTP_month (month);

	if (tms.tm_mon == -1)
		return (time_t) -1;

	// mktime is a Standard C function.
	{

#if !defined (ACE_HAS_REENTRANT_LIBC)
		ACE_MT (ACE_GUARD_RETURN (ACE_SYNCH_MUTEX, g, HTTP_Helper::mutex_, -1));
#endif /* NOT ACE_HAS_REENTRANT_LIBC */

		return ACE_OS::mktime (&tms);
	}
}

const char *
HTTP_Helper::HTTP_date (void)
{
	if (HTTP_Helper::date_string_ == 0)
	{
		ACE_MT (ACE_GUARD_RETURN (ACE_SYNCH_MUTEX, m, HTTP_Helper::mutex_, 0));

		if (HTTP_Helper::date_string_ == 0)
		{
			// 40 bytes is all I need.
			
			ACE_NEW_RETURN (HTTP_Helper::date_string_, char[40], 0);

			if (!HTTP_Helper::HTTP_date (HTTP_Helper::date_string_))
			{
				delete [] HTTP_Helper::date_string_;
				HTTP_Helper::date_string_ = 0;
			}
		}
	}

	return HTTP_Helper::date_string_;
}

const char *
HTTP_Helper::HTTP_date (char *s)
{
	// Return the date-string formatted per HTTP standards.  Time must
	// be in UTC, so using the 'strftime' call (which obeys the locale)
	// isn't correct.
	static const char* months[] = {"Jan","Feb","Mar","Apr","May","Jun",
		"Jul","Aug","Sep","Oct","Nov","Dec"};
	static const char* days[] = {"Sun","Mon","Tue","Wed","Thu","Fri","Sat"};

	time_t tloc;
	struct tm tms;
	char * date_string = s;

	if (ACE_OS::time (&tloc) != (time_t) -1
		&& ACE_OS::gmtime_r (&tloc, &tms) != 0)
	{
		ACE_OS::sprintf (date_string,
			"%s, %2.2d %s %4.4d %2.2d:%2.2d:%2.2d GMT",
			days[tms.tm_wday], tms.tm_mday, months[tms.tm_mon],
			tms.tm_year + 1900, tms.tm_hour, tms.tm_min, tms.tm_sec);
	}
	else
		date_string = 0;

	return date_string;
}

int
HTTP_Helper::HTTP_month (const char *month)
{
	for (size_t i = 0; i < 12; i++)
		if (ACE_OS::strcmp(month, HTTP_Helper::months_[i]) == 0)
			return i;

	return -1;
}

const char *
HTTP_Helper::HTTP_month (int month)
{
	if (month < 0 || month >= 12)
		return 0;

	return HTTP_Helper::months_[month];
}

// Fix the path if it needs fixing/is fixable.

char *
HTTP_Helper::HTTP_decode_string (char *path)
{
	// replace the percentcodes with the actual character
	int i, j;
	char percentcode[3];

	for (i = j = 0; path[i] != '\0'; i++, j++)
	{
		if (path[i] == '%')
		{
			percentcode[0] = path[++i];
			percentcode[1] = path[++i];
			percentcode[2] = '\0';
			path[j] = (char) ACE_OS::strtol (percentcode, (char **) 0, 16);
		}
		else
			path[j] = path[i];
	}

	path[j] = path[i];

	return path;
}

char *
HTTP_Helper::HTTP_decode_base64 (char *data)
{
	char inalphabet[256], decoder[256];

	memset (inalphabet, 0, sizeof (inalphabet));
	memset (decoder, 0, sizeof (decoder));

	for (int i = ACE_OS::strlen (HTTP_Helper::alphabet_) - 1;
		i >= 0;
		i--)
	{
		inalphabet[(unsigned int) HTTP_Helper::alphabet_[i]] = 1;
		decoder[(unsigned int) HTTP_Helper::alphabet_[i]] = i;
	}

	char *indata = data;
	char *outdata = data;

	int bits = 0;
	int c;
	int char_count = 0;
	int errors = 0;

	while ((c = *indata++) != '\0')
	{
		if (c == '=')
			break;
		if (c > 255 || ! inalphabet[c])
			continue;
		bits += decoder[c];
		char_count++;
		if (char_count == 4)
		{
			*outdata++ = (bits >> 16);
			*outdata++ = ((bits >> 8) & 0xff);
			*outdata++ = (bits & 0xff);
			bits = 0;
			char_count = 0;
		}
		else
			bits <<= 6;
	}

	if (c == '\0')
	{
		if (char_count)
		{
			ACE_DEBUG ((LM_DEBUG,
				"base64 encoding incomplete: at least %d bits truncated\n",
				((4 - char_count) * 6)));
			errors++;
		}
	}
	else
	{
		// c == '='
		switch (char_count)
		{
		case 1:
			ACE_DEBUG ((LM_DEBUG,
				"base64 encoding incomplete: at least 2 bits missing\n"));
			errors++;
			break;
		case 2:
			*outdata++ = (bits >> 10);
			break;
		case 3:
			*outdata++ = (bits >> 16);
			*outdata++ = ((bits >> 8) & 0xff);
			break;
		}
	}
	*outdata = '\0';
	return errors ? 0 : data;
}

char *
HTTP_Helper::HTTP_encode_base64 (char *data)
{
	char buf[BUFSIZ];
	int c;
	int error;
	int char_count = 0;
	int bits = 0;
	error = 0;
	char *indata = data;
	char *outdata = buf;
	const unsigned char ASCII_MAX = ~0;

	while ((c = *indata++) != '\0')
	{
		if (c > (int)ASCII_MAX)
		{
			ACE_DEBUG ((LM_DEBUG, "encountered char > 255 (decimal %d)\n", c));
			error++;
			break;
		}
		bits += c;
		char_count++;

		if (char_count == 3)
		{
			*outdata++ = HTTP_Helper::alphabet_[bits >> 18];
			*outdata++ = HTTP_Helper::alphabet_[(bits >> 12) & 0x3f];
			*outdata++ = HTTP_Helper::alphabet_[(bits >> 6) & 0x3f];
			*outdata++ = HTTP_Helper::alphabet_[bits & 0x3f];
			bits = 0;
			char_count = 0;
		}
		else
			bits <<= 8;
	}

	if (!error)
	{
		if (char_count != 0)
		{
			bits <<= 16 - (8 * char_count);
			*outdata++ = HTTP_Helper::alphabet_[bits >> 18];
			*outdata++ = HTTP_Helper::alphabet_[(bits >> 12) & 0x3f];

			if (char_count == 1)
			{
				*outdata++ = '=';
				*outdata++ = '=';
			}
			else
			{
				*outdata++ = HTTP_Helper::alphabet_[(bits >> 6) & 0x3f];
				*outdata++ = '=';
			}
		}
		*outdata = '\0';
		ACE_OS::strcpy (data, buf);
	}

	return (error ? 0 : data);
}

int
HTTP_Helper::fixyear (int year)
{
	// Fix the year 2000 problem

	if (year > 1000)
		year -= 1900;
	else if (year < 100)
	{
		struct tm tms;
		time_t tloc;

		if (ACE_OS::time (&tloc) != (time_t) -1)
		{
			ACE_OS::gmtime_r (&tloc, &tms);

			if (tms.tm_year % 100 == year)
				year = tms.tm_year;

			// The last two cases check boundary conditions, in case the
			// year just changed at the moment we checked to see if we
			// need to fix it.
			if ((year+1) % 100 == tms.tm_year % 100)
				year = tms.tm_year - 1;

			if (year == (tms.tm_year + 1) % 100)
				year = tms.tm_year + 1;

			// What to do if none of the above?
		}
	}

	return year;
}

const char **
HTTP_Status_Code::instance (void)
{
	if (HTTP_Status_Code::instance_ == 0)
	{
		ACE_MT (ACE_GUARD_RETURN (ACE_SYNCH_MUTEX, g, lock_, 0));

		if (HTTP_Status_Code::instance_ == 0)
		{
			for (size_t i = 0;
				i < HTTP_Status_Code::MAX_STATUS_CODE + 1;
				i++)
			{
				switch (i)
				{
				case STATUS_OK:
					HTTP_Status_Code::Reason[i] = "OK"; break;
				case STATUS_CREATED:
					HTTP_Status_Code::Reason[i] = "Created"; break;
				case STATUS_ACCEPTED:
					HTTP_Status_Code::Reason[i] = "Accepted"; break;
				case STATUS_NO_CONTENT:
					HTTP_Status_Code::Reason[i] = "No Content"; break;
				case STATUS_MOVED_PERMANENTLY:
					HTTP_Status_Code::Reason[i] = "Moved Permanently"; break;
				case STATUS_MOVED_TEMPORARILY:
					HTTP_Status_Code::Reason[i] = "Moved Temporarily"; break;
				case STATUS_NOT_MODIFIED:
					HTTP_Status_Code::Reason[i] = "Not Modified"; break;
				case STATUS_BAD_REQUEST:
					HTTP_Status_Code::Reason[i] = "Bad Request"; break;
				case STATUS_UNAUTHORIZED:
					HTTP_Status_Code::Reason[i] = "Unauthorized"; break;
				case STATUS_FORBIDDEN:
					HTTP_Status_Code::Reason[i] = "Forbidden"; break;
				case STATUS_NOT_FOUND:
					HTTP_Status_Code::Reason[i] = "Not Found"; break;
				case STATUS_INTERNAL_SERVER_ERROR:
					HTTP_Status_Code::Reason[i] = "Internal Server Error"; break;
				case STATUS_NOT_IMPLEMENTED:
					HTTP_Status_Code::Reason[i] = "Not Implemented"; break;
				case STATUS_BAD_GATEWAY:
					HTTP_Status_Code::Reason[i] = "Bad Gateway"; break;
				case STATUS_SERVICE_UNAVAILABLE:
					HTTP_Status_Code::Reason[i] = "Service Unavailable"; break;
				default:
					HTTP_Status_Code::Reason[i] = "Unknown";
				}
			}

			HTTP_Status_Code::instance_ = 1;
		}

		// GUARD released
	}

	return HTTP_Status_Code::Reason;
}

//////////////////////////////////////////////////////////////////////////

Headers::Headers (void) : done_(0)
{
}

Headers::~Headers (void)
{
}

void
Headers::recognize (const char * const header)
{
	(void)this->map_[header];
}

void
Headers::parse_header_line (char * const header_line)
{
	char *ptr = header_line;
	char *buf = header_line;
	int offset = 1;

	ptr = ACE_OS::strchr (header_line, '\n');

	if (ptr > header_line && ptr[-1] == '\r')
	{
		ptr--;
		offset++;
	}

	if (ptr == header_line)
	{
		this->done_ = 1;
		return;
	}

	*ptr = '\0';
	ptr += offset;

	char *value = 0;
	char *header = ACE_OS::strtok_r (buf, ":", &value);

	/*ACE_DEBUG((LM_DEBUG, " (%t) Headers::parse_header_line [%s]\n",
		header ? header : "<empty>"));*/

	if (header != 0 && this->map_.mapped (header))
	{
		while (ACE_OS::ace_isspace (*value))
			value++;

		this->map_[header] = value;

		/*ACE_DEBUG((LM_DEBUG, " (%t) Headers::parse_header_line <%s>\n",
			value ? value : "<empty>"));*/
	}

	// Write back the unused portion of the input.
	ACE_OS::memmove (header_line, ptr, ACE_OS::strlen(ptr) + 1);
}

int
Headers::complete_header_line (char *const header_line)
{
	// Algorithm --
	// Scan for end of line marker.
	// If the next character is linear white space, then unfold the header.
	// Else, if the next character is printable, we have a complete header line.
	// Else, presumably the next character is '\0', so the header is incomplete.

	// return -1 if end of line but not complete header line
	// return 0 if no end of line marker
	// return 1 if complete header line

	char *ptr = header_line;
	int offset;

	if (!this->end_of_line (ptr, offset))
		return 0;

	if (ptr == header_line)
	{
		ACE_OS::memmove (ptr, ptr+offset, ACE_OS::strlen (ptr + offset) + 1);
		this->done_ = 1;
		//ACE_DEBUG ((LM_DEBUG, "  (%t) no more headers\n"));
		return 0;
	}

	do
	{
		switch (ptr[offset])
		{
		case ' ':
		case '\t':
			ACE_OS::memmove (ptr, ptr+offset, ACE_OS::strlen (ptr + offset) + 1);
			break;

		case '\n':
		case '\r':
			return 1;

		default:
			if (ACE_OS::ace_isalpha (ptr[offset]))
				return 1;
			else
				return -1;
		}
	}
	while (this->end_of_line (ptr, offset) != 0);

	return 0;
}

int
Headers::end_of_headers (void) const
{
	return this->done_;
}

Headers_Map_Item &
Headers::operator[] (const char * const header)
{
	return this->map_[header];
}

const Headers_Map_Item &
Headers::operator[] (const char * const header) const
{
	return this->map_[header];
}

int
Headers::end_of_line (char *&line, int &offset) const
{
	char *old_line = line;
	char *ptr = ACE_OS::strchr (old_line, '\n');

	if (ptr == 0)
		return 0;

	line = ptr;
	offset = 1;

	if (line > old_line
		&& line[-1] == '\r')
	{
		line--;
		offset = 2;
	}

	return 1;
}


// Implementation of class Headers_Map

Headers_Map::Headers_Map (void)
: num_headers_(0)
{
}

Headers_Map::~Headers_Map (void)
{
}

Headers_Map_Item::Headers_Map_Item (void)
: header_(0),
value_(0)
{
}

Headers_Map_Item::~Headers_Map_Item (void)
{
	free ((void *) this->header_);
	free ((void *) this->value_);
	this->header_ = this->value_ = 0;
}

// Headers_Map_Item::operator const char * (void) const
// {
//   return this->value_ == 0 ? this->no_value_ : this->value_;
// }

Headers_Map_Item &
Headers_Map_Item::operator= (char * value)
{
	free ((void *) this->value_);
	this->value_ = ACE_OS::strdup (value);
	return *this;
}

Headers_Map_Item &
Headers_Map_Item::operator= (const char * value)
{
	free ((void *) this->value_);
	this->value_ = strdup (value);
	return *this;
}

Headers_Map_Item &
Headers_Map_Item::operator= (const Headers_Map_Item & mi)
{
	free ((void *) this->value_);
	free ((void *) this->header_);
	this->header_ = ACE_OS::strdup (mi.header_);
	this->value_ = (mi.value_ ? ACE_OS::strdup (mi.value_) : 0);
	return *this;
}

const char *
Headers_Map_Item::header (void) const
{
	return this->header_;
}

const char *
Headers_Map_Item::value (void) const
{
	return this->value_;
}

Headers_Map_Item &
Headers_Map::operator[] (const char * const header)
{
	Headers_Map_Item *item_ptr;

	item_ptr = this->find (header);

	if (item_ptr == 0)
		item_ptr = this->place (header);

	return *item_ptr;
}

const Headers_Map_Item &
Headers_Map::operator[] (const char * const header) const
{
	Headers_Map_Item *item_ptr;
	Headers_Map *mutable_this = (Headers_Map *)this;

	item_ptr = this->find (header);

	if (item_ptr == 0)
		item_ptr = mutable_this->place (header);

	return *item_ptr;
}

int
Headers_Map::mapped (const char * const header) const
{
	int result = this->find (header) != 0;

	return result;
}

Headers_Map_Item *
Headers_Map::find (const char * const header) const
{
	Headers_Map *const mutable_this = (Headers_Map *) this;

	mutable_this->garbage_.header_ = header;
#if 0
	Headers_Map_Item *mi_ptr = (Headers_Map_Item *)
		ACE_OS::bsearch (&this->garbage_,
		this->map_,
		this->num_headers_,
		sizeof (Headers_Map_Item),
		Headers_Map::compare);
#else
	int i = 0;
	int j = this->num_headers_;

	while (i < j-1)
	{
		int k = (i+j)/2;
		if (Headers_Map::compare (&this->garbage_, this->map_+k) < 0)
			j = k;
		else
			i = k;
	}

	Headers_Map_Item *mi_ptr = mutable_this->map_ + i;
	if (Headers_Map::compare (&this->garbage_, mi_ptr) != 0)
		mi_ptr = 0;
#endif

	mutable_this->garbage_.header_ = 0;

	return mi_ptr;
}

Headers_Map_Item *
Headers_Map::place (const char *const header)
{
	this->garbage_.header_ = ACE_OS::strdup (header);

	int i = this->num_headers_++;
	free ((void *) this->map_[i].header_);
	free ((void *) this->map_[i].value_);
	this->map_[i].header_ = 0;
	this->map_[i].value_ = 0;
	Headers_Map_Item temp_item;

	while (i > 0)
	{
		if (Headers_Map::compare (&this->garbage_,
			&this->map_[i - 1]) > 0)
			break;

		this->map_[i].header_ = this->map_[i - 1].header_;
		this->map_[i].value_ = this->map_[i - 1].value_;
		this->map_[i - 1].header_ = 0;
		this->map_[i - 1].value_ = 0;

		i--;
	}

	this->map_[i].header_ = this->garbage_.header_;
	this->map_[i].value_ = this->garbage_.value_;

	this->garbage_.header_ = 0;

	return &this->map_[i];
}

int
Headers_Map::compare (const void *item1,
					  const void *item2)
{
	Headers_Map_Item *a, *b;
	int result;

	a = (Headers_Map_Item *) item1;
	b = (Headers_Map_Item *) item2;

	if (a->header_ == 0 || b->header_ == 0)
	{
		if (a->header_ == 0 && b->header_ == 0)
			result = 0;
		else if (a->header_ == 0)
			result = 1;
		else
			result = -1;
	}
	else
		result = ACE_OS::strcasecmp (a->header_, b->header_);

	return (result < 0) ? -1 : (result > 0);
}


//////////////////////////////////////////////////////////////////////////

const char *const
HTTP_Request::static_header_strings_[HTTP_Request::NUM_HEADER_STRINGS] =
{
	"Date",
	"Pragma",
	"Authorization",
	"From",
	"If-Modified-Since",
	"Referrer",
	"User-Agent",
	"Allow",
	"Content-Encoding",
	"Content-Length",
	"Content-Type",
	"Expires",
	"Last-Modified"
};

const char *const
HTTP_Request::static_method_strings_[HTTP_Request::NUM_METHOD_STRINGS] =
{
	"GET",
	"HEAD",
	"POST",
	"PUT"
};

// For reasons of efficiency, this class expects buffer to be
// null-terminated, and buflen does NOT include the \0.

HTTP_Request::HTTP_Request (void)
: got_request_line_ (0),
method_ (0),
version_ (0),
header_strings_ (HTTP_Request::static_header_strings_),
method_strings_ (HTTP_Request::static_method_strings_)
{

	for (size_t i = 0;
		i < HTTP_Request::NUM_HEADER_STRINGS;
		i++)
		this->headers_.recognize (this->header_strings_[i]);
}

HTTP_Request::~HTTP_Request (void)
{
	free (this->method_);
	free (this->version_);

}

int
HTTP_Request::parse_request (ACE_Message_Block &mb)
{
	mb.wr_ptr ()[0] = '\0';

	// Note that RFC 822 does not mention the maximum length of a header
	// line.  So in theory, there is no maximum length.

	// In Apache, they assume that each header line should not exceed
	// 8K.

	int result = this->headers_.complete_header_line (mb.rd_ptr ());

	if (result != 0)
	{
		if (!this->got_request_line ())
		{
			this->parse_request_line (mb.rd_ptr ());
			while (this->headers_.complete_header_line (mb.rd_ptr ()) > 0)
				this->headers_.parse_header_line (mb.rd_ptr ());
		}
		else if (result > 0)
			do
		this->headers_.parse_header_line (mb.rd_ptr ());
		while (this->headers_.complete_header_line (mb.rd_ptr ()) > 0);
	}

	mb.wr_ptr (ACE_OS::strlen(mb.rd_ptr ()) - mb.length ());

	if (this->headers_.end_of_headers ()
		|| (this->got_request_line () && this->version () == 0))
		return this->init (mb.rd_ptr (), mb.length ());
	else
		return 0;
}

void
HTTP_Request::parse_request_line (char *const request_line)
{
	char *ptr = request_line;
	char *buf = request_line;
	int offset = 1;

	this->status_ = HTTP_Status_Code::STATUS_OK;

	ptr = ACE_OS::strchr (request_line, '\n');

	if (ptr > request_line && ptr[-1] == '\r')
		ptr--, offset++;

	if (ptr == request_line)
	{
		this->status_ = HTTP_Status_Code::STATUS_BAD_REQUEST;
		return;
	}

	*ptr = '\0';
	ptr += offset;

	char *lasts = 0; // for strtok_r

	// Get the request type.
	this->got_request_line_ = 1;

	if (this->method (ACE_OS::strtok_r (buf, " \t", &lasts)))
	{
		ACE_OS::strtok_r (0, " \t", &lasts); ///跳过
		this->type (this->method ());

		if (this->version (ACE_OS::strtok_r (0, " \t", &lasts)) == 0
			&& this->type () != HTTP_Request::GET)
			this->status_ = HTTP_Status_Code::STATUS_NOT_IMPLEMENTED;
	}

	//ACE_DEBUG ((LM_DEBUG, " (%t) request %s %s parsed\n",
	//	(this->method () ? this->method () : "-"),
	//	(this->version () ? this->version () : "HTTP/0.9")));

	memmove (buf, ptr, strlen (ptr)+1);
}

int
HTTP_Request::init (char *const buffer,
					int buflen)
{
	// Initialize these every time.
	content_length_ = -1;

	// Extract the data pointer.
	data_ = buffer;
	datalen_ = 0;

	// Set the datalen
	if (data_ != 0)
		datalen_ = buflen;
	else
		datalen_ = 0;

	//ACE_DEBUG ((LM_DEBUG, " (%t) init has initialized\n"));

	return 1;
}

const char *
HTTP_Request::method (void) const
{
	return this->method_;
}



const char *
HTTP_Request::version (void) const
{
	return this->version_;
}

int
HTTP_Request::got_request_line (void) const
{
	return this->got_request_line_;
}

int
HTTP_Request::type (void) const
{
	return type_;
}

const Headers &
HTTP_Request::headers (void) const
{
	return this->headers_;
}

const char *
HTTP_Request::header_strings (int index) const
{
	const char *hs = 0;

	if (0 <= index && index < NUM_HEADER_STRINGS)
		hs = this->header_strings_[index];

	return hs;
}

const char *
HTTP_Request::header_values (int index) const
{
	const char *hs = 0;
	const char *hv = 0;

	if (0 <= index && index < NUM_HEADER_STRINGS)
	{
		hs = this->header_strings_[index];
		hv = this->headers_[hs].value ();
	}

	return hv;
}

char *
HTTP_Request::data (void)
{
	return data_;
}

int
HTTP_Request::data_length (void)
{
	return datalen_;
}

int
HTTP_Request::content_length (void)
{
	if (this->content_length_ == -1)
	{
		const char * clv = this->headers_["Content-length"].value ();
		this->content_length_ = (clv ? ACE_OS::atoi (clv) : 0);
	}

	return this->content_length_;
}

int
HTTP_Request::status (void)
{
	return this->status_;
}

const char *
HTTP_Request::status_string (void)
{
	return HTTP_Status_Code::instance ()[this->status_];
}

void
HTTP_Request::dump (void)
{
	ACE_DEBUG ((LM_DEBUG, "%s command.\n"
		" length of the file is %d,"
		" data string is %s,"
		" datalen is %d,"
		" status is %d, which is %s\n\n",
		this->method () ? this->method () : "EMPTY",
		this->content_length (),
		this->data () ? this->data () : "EMPTY",
		this->data_length (),
		this->status (),
		this->status_string ()));
}

const char *
HTTP_Request::method (const char *method_string)
{
	if (this->method_)
		free (this->method_);

	if (method_string == 0)
	{
		this->status_ = HTTP_Status_Code::STATUS_BAD_REQUEST;
		this->method_ = 0;
	}
	else
		this->method_ = ACE_OS::strdup (method_string);

	return this->method_;
}

const char *
HTTP_Request::version (const char *version_string)
{
	if (this->version_)
		free (this->version_);

	if (version_string)
		this->version_ = ACE_OS::strdup (version_string);
	else
		this->version_ = 0;

	return this->version_;
}

int
HTTP_Request::type (const char *type_string)
{
	this->type_ = HTTP_Request::NO_TYPE;

	if (type_string == 0)
		return this->type_;

	for (size_t i = 0;
		i < HTTP_Request::NUM_METHOD_STRINGS;
		i++)

		if (ACE_OS::strcmp (type_string, this->method_strings_[i]) == 0)
		{
			this->type_ = i;
			break;
		}

		if (this->type_ == HTTP_Request::NO_TYPE)
			this->status_ = HTTP_Status_Code::STATUS_NOT_IMPLEMENTED;

		return this->type_;
}