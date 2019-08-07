import store from '../store'
import * as signalR from "@aspnet/signalr";

let conn = null;
let USERACCOUNT = store.state.user.UserAccount;
let USERNAME = store.state.user.UserName;
let USERID = store.state.user.UserId;
let ACCESSTOKEN = store.state.auth.access_token;
export default {
    install(Vue, options) {
        Vue.prototype.$signalr = Vue.signalr = this;
    },
    getConnect: function() {
        return conn;
    },
    openConnect: function(url) {
        conn = new signalR.HubConnectionBuilder().withUrl(url, {
            accessTokenFactory: () => ACCESSTOKEN,
            transport: signalR.HttpTransportType.WebSockets
        }).configureLogging(signalR.LogLevel.Information).build();
        if (USERID != null && USERID.length > 0) {
            conn.id = USERID;
        }
        conn.start().catch(function(err) {
            return console.error(err.toString());
        });
    },
    closeConnect: function() {
        conn.stop().catch(function(err) {
            return console.error(err.toString());
        });
    },
    sendMessage: function(mssg) {
        conn.invoke("SendMessage", USERACCOUNT, USERNAME, mssg).catch(function(err) {
            return console.error(err.toString());
        });
    },
    sendMessageToUser: function(mssg, receiveid) {
        conn.invoke("SendMessageToUser", USERACCOUNT, USERNAME, receiveid, mssg).catch(function(err) {
            return console.error(err.toString());
        });
    }
}