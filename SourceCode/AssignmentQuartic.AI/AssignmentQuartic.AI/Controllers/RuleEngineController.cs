using AssignmentQuartic.AI.Models;
using MDMWebApi.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AssignmentQuartic.AI.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("quartic/api")]
    public class RuleEngineController : ApiController
    {
        [HttpPost]
        [Route("buildrule")]
        public HttpResponseMessage Fn_BuildRule(Rules _rule)
        {
            string _ErrorMessage = "";
            try
            {
                if (_rule == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, " Json format incorrect");
                }

                if(WebApiConfig._ObjDB.fn_ModifyRule(_rule,0,ref _ErrorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.Created,"success" );
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fail");
                }
                                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, " Json format incorrect");
            }
        }

        [HttpGet]
        [Route("getrule")]
        public HttpResponseMessage Fn_GetRule()
        {
            string _ErrorMessage = "";
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, WebApiConfig._ObjDB.fn_GetRules(ref _ErrorMessage));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, " Json format incorrect");
            }
        }

        [HttpPut]
        [Route("updaterule")]
        public HttpResponseMessage Fn_UpdateRule(Rules _rule)
        {
            string _ErrorMessage = "";
            try
            {
                if (_rule == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, " Json format incorrect");
                }

                if (WebApiConfig._ObjDB.fn_ModifyRule(_rule, 1, ref _ErrorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, "success");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fail");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, " Json format incorrect");
            }
        }

        [HttpDelete]
        [Route("deleterule/{Rule_ID}")]
        public HttpResponseMessage Fn_DeleteRule(int Rule_ID)
        {
            string _ErrorMessage = "";
            try
            {
                if (WebApiConfig._ObjDB.fn_ModifyRule(null, 2, ref _ErrorMessage,Rule_ID))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, "success");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fail");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, " Json format incorrect");
            }

        }

        [HttpPost]
        [Route("validatestream")]
        public HttpResponseMessage Fn_ValidateStream(List<RootObject> _LstRootObject)
        {
            string _ErrorMessage = "";
            try
            {
                if (_LstRootObject == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, " Json format incorrect ");
                }

                if (WebApiConfig._ObjDB.fn_Insert(_LstRootObject, ref _ErrorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, " success ");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, " Fail ");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            
        }

        [HttpGet]
        [Route("getviolationreport")]
        public HttpResponseMessage Fn_GetViolationReport()
        {
            string _ErrorMessage = "";
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, WebApiConfig._ObjDB.fn_GetViolationdata(ref _ErrorMessage));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            
        }


    }
}
