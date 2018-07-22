using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EmailSystem
{
	public event RequestEventHandler<bool> mailEnven;
	public event RequestEventHandler<COM_Mail> UpdatemailEnven;
    private List<COM_Mail> _mails = new List<COM_Mail>();
	private static EmailSystem _instance;
	public static EmailSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new EmailSystem();
			return _instance;
		}
	}

    public void AppendMail(COM_Mail[] mails)
    {
        _mails.AddRange(mails);
        if (mailEnven != null)
            mailEnven(false);
    }

	public COM_Mail[] Mails
	{
		get
		{
			return _mails.ToArray();
		}

	}

	public void updateMailOk(COM_Mail mail)
	{
		for(int i =0;i<_mails.Count;i++ )
		{
			if(_mails[i].mailId_ == mail.mailId_)
			{
				_mails[i]= mail;
				break;
			}
		}

		if(UpdatemailEnven != null)
		{
			UpdatemailEnven(mail);
		}
	}

	public void DelMial(int id)
	{
        for (int i = 0; i < _mails.Count; i++)
		{
			if(Mails[i] == null)
				continue;
			if(Mails[i].mailId_ == id)
			{
				_mails.Remove(Mails[i]);
				break;
			}
		}
	}


    public void Clear()
    {
        _mails.Clear();
    }

}