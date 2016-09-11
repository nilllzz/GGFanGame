﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Threading;

namespace GGFanGame.GameJolt.API
{
    /// <summary>
    /// A request to the GameJolt API.
    /// </summary>
    internal sealed partial class GameJoltRequest
    {
        const string GAMEID = ""; //TODO: Put Game Id from GameJolt here.
        const string GAMEKEY = ""; //TODO: Put Game key from GameJolt here.
        const string HOST = "http://api.gamejolt.com/api/game/";
        const string API_VERSION = "v1_1";

        public delegate void FinishedEventHandler(RequestResult result);

        /// <summary>
        /// The event that gets fired once the request finished.
        /// </summary>
        public event FinishedEventHandler Finished;

        private readonly RequestType _requestType;
        private readonly string _endPoint;
        private string _postData;
        private RequestFormat _returnFormat;
        private RequestResult _requestResult;

        private readonly Dictionary<string, string> _urlParameters;

        /// <summary>
        /// Adds an URL parameter to the parameter dictionary.
        /// </summary>
        private void addUrlParameter(string key, string value)
        {
            _urlParameters.Add(key, value);
        }

        private GameJoltRequest(RequestType requestType, string endpoint)
        {
            _urlParameters = new Dictionary<string, string>();

            _requestType = requestType;
            _endPoint = endpoint;

            if (!_endPoint.StartsWith("/"))
            {
                _endPoint = "/" + _endPoint;
            }
            if (!_endPoint.EndsWith("/"))
            {
                _endPoint = _endPoint + "/";
            }

            addUrlParameter("game_id", GAMEID);
        }

        /// <summary>
        /// Executes the constructed API request.
        /// </summary>
        public RequestResult execute(RequestFormat returnFormat)
        {
            _returnFormat = returnFormat;

            executeInternal();

            return _requestResult;
        }

        /// <summary>
        /// Executes the constructed API request asynchronously.
        /// </summary>
        public void executeAsync(RequestFormat returnFormat)
        {
            _returnFormat = returnFormat;

            var t = new Thread(executeInternal) { IsBackground = true };
            t.Start();
        }

        private void executeInternal()
        {
            var url = getURL();

            if (_requestType == RequestType.GET) //GET
            {
                try
                {
                    var getRequest = (HttpWebRequest)WebRequest.Create(url);
                    getRequest.Method = "GET";

                    var getResponse = (HttpWebResponse)getRequest.GetResponse();

                    var resultData = new StreamReader(getResponse.GetResponseStream()).ReadToEnd();

                    _requestResult = new RequestResult(RequestType.GET, RequestStatus.Success, resultData);
                }
                catch (Exception ex)
                {
                    _requestResult = new RequestResult(RequestType.GET, new RequestException(ex));
                }
            }
            else //POST
            {
                try
                {
                    var postContent = "data=" + _postData;

                    var postRequest = (HttpWebRequest)WebRequest.Create(url);
                    postRequest.AllowWriteStreamBuffering = true;
                    postRequest.Method = "POST";
                    postRequest.ContentLength = postContent.Length;
                    postRequest.ContentType = "application/x-www-form-urlencoded";
                    postRequest.ServicePoint.Expect100Continue = true;

                    var postWriter = new StreamWriter(postRequest.GetRequestStream());
                    postWriter.Write(postContent);
                    postWriter.Close();

                    var postResponse = (HttpWebResponse)postRequest.GetResponse();

                    var resultData = new StreamReader(postResponse.GetResponseStream()).ReadToEnd();
                    _requestResult = new RequestResult(RequestType.POST, RequestStatus.Success, resultData);

                }
                catch (Exception ex)
                {
                    _requestResult = new RequestResult(RequestType.POST, new RequestException(ex));
                }
            }

            Finished?.Invoke(_requestResult);
        }

        private string getURL()
        {
            //Construct URL first:

            var urlSB = new StringBuilder(HOST + API_VERSION + _endPoint);

            //Append format to url:
            addUrlParameter("format", _returnFormat.ToString().ToLower());

            //Append the url parameters to the string builder:
            for (var i = 0; i < _urlParameters.Count; i++)
            {
                if (i == 0)
                    urlSB.Append("?");
                else
                    urlSB.Append("&");

                urlSB.Append(_urlParameters.Keys.ElementAt(i) + "=" +
                                Networking.UrlEncoder.encode(_urlParameters.Values.ElementAt(i)));
            }

            var url = urlSB.ToString();

            // append signature to URL:
            url += "&signature=" + getUrlSignature(url);

            return url;
        }

        private static string getUrlSignature(string url)
        {
            //compute hash for signature:
            var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(url + GAMEKEY));
            var signatureSB = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                signatureSB.Append(data[i].ToString("x2"));
            }

            return signatureSB.ToString();
        }

    }

    /// <summary>
    /// A class that contains the result of an API request.
    /// </summary>
    internal sealed class RequestResult
    {
        private readonly RequestStatus _requestStatus;
        private readonly string _requestData;
        private readonly RequestType _requestType;

        private readonly RequestException _exception;

        /// <summary>
        /// Creates a new instance of the RequestResult class.
        /// </summary>
        /// <param name="requestType">The type of the request.</param>
        /// <param name="status">The status of the request.</param>
        /// <param name="data">The result data of the request.</param>
        public RequestResult(RequestType requestType, RequestStatus status, string data)
        {
            _requestType = requestType;
            _requestStatus = status;
            _requestData = data;
        }

        /// <summary>
        /// Creates a new instance of the RequestResult class for when the request failed.
        /// </summary>
        /// <param name="requestType">The type of the request.</param>
        /// <param name="exception">The exception that occurred.</param>
        public RequestResult(RequestType requestType, RequestException exception) : this(requestType, RequestStatus.Failure, "")
        {
            _exception = exception;
        }

        /// <summary>
        /// The result data of the request.
        /// </summary>
        public string data => _requestData;

        /// <summary>
        /// The status of the request.
        /// </summary>
        public RequestStatus status => _requestStatus;

        /// <summary>
        /// The type of the request.
        /// </summary>
        public RequestType requestType => _requestType;

        /// <summary>
        /// An exception that might have occured during the request. This is null when the request was a success.
        /// </summary>
        public RequestException exception => _exception;
    }

    /// <summary>
    /// An exception that occured during an API request.
    /// </summary>
    internal sealed class RequestException : Exception
    {
        public RequestException(Exception innerException) : base("A problem occured while making a request to the GameJolt API.", innerException) { }
    }
}