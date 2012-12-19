using System;
using System.Globalization;
using NLog;

namespace Easis.Wfs
{
    /// <summary>
    /// NLog logger wrapper provides WfId param to each log message.
    /// </summary>
    public class WfLog
    {
        // -------------------------------------------------------
        public void ErrorException(string message, Exception exception)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Error, _log.Name, _ci, message, new object[] { }, exception);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Error(string message)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Error, _log.Name, message);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Error(string message, params object[] args)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Error, _log.Name, _ci, message, args);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        // -------------------------------------------------------

        public void Trace(string message)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Trace, _log.Name, message);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Trace(string message, params object[] args)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Trace, _log.Name, _ci, message, args);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void TraceException(string message, Exception exception)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Trace, _log.Name, _ci, message, new object[] { }, exception);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        // -------------------------------------------------------
        public void Debug(string message)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Debug, _log.Name, message);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Debug(string message, params object[] args)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Debug, _log.Name, _ci, message, args);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void DebugException(string message, Exception exception)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Debug, _log.Name, _ci, message, new object[] { }, exception);

            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }
        // -------------------------------------------------------

        public void Info(string message)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, _log.Name, message);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Info(string message, params object[] args)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, _log.Name, _ci, message, args);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void InfoException(string message, Exception exception)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, _log.Name, _ci, message, new object[] { }, exception);

            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }
        // -------------------------------------------------------

        public void WarnException(string message, Exception exception)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Warn, _log.Name, _ci, message, new object[] { }, exception);

            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Warn(string message)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Warn, _log.Name, message);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Warn(string message, params object[] args)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Warn, _log.Name, _ci, message, args);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }
        // -------------------------------------------------------

        public void Fatal(string message)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Fatal, _log.Name, message);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void Fatal(string message, params object[] args)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Fatal, _log.Name, _ci, message, args);
            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }

        public void FatalException(string message, Exception exception)
        {
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Fatal, _log.Name, _ci, message, new object[] { }, exception);

            theEvent.Properties["WfId"] = _wfId;
            _log.Log(theEvent);
        }
        // -------------------------------------------------------

        private readonly Logger _log;
        private readonly string _wfId;
        private readonly CultureInfo _ci = CultureInfo.CurrentCulture;

        public Logger Log { get { return _log; } }
        public string WfId { get { return _wfId; } }

        public WfLog(WfLog from, Logger newlog)
        {
            this._wfId = from.WfId;
            _log = newlog;
        }

        public WfLog(Logger log, Guid WfId)
        {
            _log = log;
            _wfId = WfId.ToString();
        }
    }
}