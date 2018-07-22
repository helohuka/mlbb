#ifndef __HTTP_PARSER_H__
#define __HTTP_PARSER_H__
#include "config.h"


class HTTP_Helper
{
public:

  // Convert and HTTP-date into a time_t
  static time_t HTTP_mktime (const char *httpdate);

  // Create today's date
  static const char *HTTP_date (void);
  static const char *HTTP_date (char *s);

  // Month conversions (ascii <--> numeric)
  static int HTTP_month (const char *month);
  static const char *HTTP_month (int month);

  static char *HTTP_decode_string (char *path);

  // Encode/Decode base64 stuff (weak security model)
  static char *HTTP_decode_base64 (char *data);
  static char *HTTP_encode_base64 (char *data);

private:

  static int fixyear (int year);

private:
  static const char *const months_[12];
  static char const *alphabet_;

  /// Use this sometimes (e.g. HTTP_date)
  static char *date_string_;
  static ACE_SYNCH_MUTEX mutex_;
};

// Design around the Singleton pattern

/**
 * @class HTTP_Status_Code
 *
 * @brief Go from numeric status codes to descriptive strings.
 *
 * Design around the Singleton pattern
 */
class HTTP_Status_Code
{
public:
  /// Singleton access point.
  static const char **instance (void);

  enum STATUS_CODE
  {
    STATUS_OK = 200,
    STATUS_CREATED = 201,
    STATUS_ACCEPTED = 202,
    STATUS_NO_CONTENT = 204,
    STATUS_MOVED_PERMANENTLY = 301,
    STATUS_MOVED_TEMPORARILY = 302,
    STATUS_NOT_MODIFIED = 304,
    STATUS_BAD_REQUEST = 400,
    STATUS_UNAUTHORIZED = 401,
    STATUS_FORBIDDEN = 403,
    STATUS_NOT_FOUND = 404,
    STATUS_INTERNAL_SERVER_ERROR = 500,
    STATUS_NOT_IMPLEMENTED = 501,
    STATUS_BAD_GATEWAY = 502,
    STATUS_SERVICE_UNAVAILABLE = 503,
    STATUS_INSUFFICIENT_DATA = 399
  };

  enum
  {
    MAX_STATUS_CODE = 599
  };

private:
  // Singleton pattern is afoot here.
  static const char *Reason[MAX_STATUS_CODE + 1];
  static int instance_;
  static ACE_SYNCH_MUTEX lock_;
};


class Headers_Map_Item
{
friend class Headers_Map;
friend class Headers;

private:
  Headers_Map_Item (void);
  ~Headers_Map_Item (void);

  // operator const char * (void) const;
  Headers_Map_Item &operator= (char *);
  Headers_Map_Item &operator= (const char *);
  Headers_Map_Item &operator= (const Headers_Map_Item &);

public:
  const char *header (void) const;
  const char *value (void) const;

private:
  const char *header_;
  const char *value_;
};

/**
 * @class Headers_Map
 *
 * @brief Map textual headings to header values (e.g. "Subject:" maps to
 * "Re: My left foot"
 */
class Headers_Map
{
public:
  Headers_Map (void);
  ~Headers_Map (void);

  Headers_Map_Item &operator[] (const char *const header);
  const Headers_Map_Item &operator[] (const char *const header) const;

  enum
  {
    MAX_HEADERS = 100
  };

  int mapped (const char *const header) const;

private:
  Headers_Map_Item *find (const char *const header) const;
  Headers_Map_Item *place (const char *const header);
  static int compare (const void *item1, const void *item2);

private:
  Headers_Map_Item map_[MAX_HEADERS];
  Headers_Map_Item garbage_;

  int num_headers_;
};

/**
 * @class Headers
 *
 * @brief A general mechanism to parse headers of Internet text headers.
 *
 * Allow interesting headers to be inserted and later associated
 * with values.  This implementation assumes the parsing of headers
 * will be done from ACE_Message_Blocks.
 */
class Headers
{
public:
  Headers (void);
  ~Headers (void);

  void recognize (const char *const header);

  void parse_header_line (char *const header_line);

  /**
   * -1 -> end of line but not complete header line
   *  0 -> no end of line marker
   *  1 -> complete header line
   */
  int complete_header_line (char *const header_line);

  int end_of_headers (void) const;

  enum
  {
    MAX_HEADER_LINE_LENGTH = 8192
  };

  Headers_Map_Item &operator[] (const char *const header);
  const Headers_Map_Item &operator[] (const char *const header) const;

private:
  int end_of_line (char *&line, int &offset) const;

private:
  Headers_Map map_;
  int done_;
};
class HTTP_Request
{
public:
	/// Default construction.
	HTTP_Request (void);

	/// Destructor.
	~HTTP_Request (void);

	/// parse an incoming request
	int parse_request (ACE_Message_Block &mb);

	/// the first line of a request is the request line, which is of the
	/// form: METHOD URI VERSION.
	void parse_request_line (char *const request_line);

	/// Initialize the request object.  This will parse the buffer and
	/// prepare for the accessors.
	int init (char *const buffer,
		int buflen);

public:
	// = The Accessors.

	/// HTTP request method
	const char *method (void) const;

	/// HTTP request version
	const char *version (void) const;

	/// The cgi request query string
	const char *query_string (void) const;

	/// The cgi request path information
	const char *path_info (void) const;

	/// The type of the HTTP request
	int type (void) const;

	/// The headers that were parsed from the request
	const Headers &headers (void) const;

	/// Header strings stored
	const char *header_strings (int index) const;

	/// Values associated with the header strings
	const char *header_values (int index) const;

	/// The buffer into which request data is read
	char *data (void);

	/// The length of the request data
	int data_length (void);

	/// The length of incoming content if any
	int content_length (void);

	/// Current status of the incoming request
	int status (void);

	/// A string describing the state of the incoming request
	const char *status_string (void);

	/// Dump the state of the request.
	void dump (void);

	enum
	{
		NO_TYPE = -1,
		GET = 0,
		HEAD,
		POST,
		PUT,
		NUM_METHOD_STRINGS
	};
	// Values for request type

	enum
	{
		DATE = 0,
		PRAGMA,
		AUTHORIZATION,
		FROM,
		IF_MODIFIED_SINCE,
		REFERRER,
		USER_AGENT,
		ALLOW,
		CONTENT_ENCODING,
		CONTENT_LENGTH,
		CONTENT_TYPE,
		EXPIRES,
		LAST_MODIFIED,
		NUM_HEADER_STRINGS
	};
	// Header strings

private:
	// = Private Accessors which can set values
	const char *method (const char *method_string);
	const char *version (const char *version_string);

	int type (const char *type_string);

private:
	int got_request_line (void) const;

private:
	int got_request_line_;
	Headers headers_;

	char *method_;
	char *version_;

	const char * const *const header_strings_;
	static const char *const static_header_strings_[NUM_HEADER_STRINGS];

	const char * const *const method_strings_;
	static const char *const static_method_strings_[NUM_METHOD_STRINGS];

	char *data_;
	int datalen_;
	int content_length_;
	int status_;
	int type_;
};

#endif