using Microsoft.AspNetCore.Http;

public class SessionManager
{
    private readonly ISession _session;
    private const string SESSION_KEY = "_SessionKey";
    private const string SESSION_IP = "_SessionIP";
    private const string SESSION_LANGUAGE = "_SessionLanguage";
    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext.Session;
    }


    public string SessionKey
    {
        get
        {
            return _session.GetString(SESSION_KEY);
        }
        set
        {
            _session.SetString(SESSION_KEY, value);
        }
    }

    public string SessionIP
    {
        get
        {
            return _session.GetString(SESSION_IP);
        }
        set
        {
            _session.SetString(SESSION_IP, value);
        }
    }
    public string SessionLanguage
    {
        get
        {
            return _session.GetString(SESSION_LANGUAGE);
        }
        set
        {
            _session.SetString(SESSION_LANGUAGE, value);
        }
    }

    //Clears user session data on logout
    public void ClearSessions()
    {
        _session.Remove("_SessionKey");
    }

}