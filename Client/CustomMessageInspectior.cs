using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CustomMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // No implementation needed
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var httpRequestMessageProperty = new HttpRequestMessageProperty();
            var currentUser = SessionManager.GetCurrentUser();
            if (currentUser != null)
            {
                httpRequestMessageProperty.Headers["UserId"] = currentUser.UserId.ToString();
                httpRequestMessageProperty.Headers["Role"] = currentUser.Role.ToString();
            }
            request.Properties[HttpRequestMessageProperty.Name] = httpRequestMessageProperty;
            return null;
        }
    }
}
