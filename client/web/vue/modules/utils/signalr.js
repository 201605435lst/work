import config from '@/config';
import Vue from 'vue';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  HubInvocationMessage,
  HttpTransportType,
} from '@aspnet/signalr';
let vm = new Vue({});
const checkJsonString = function (str) {
  if (typeof str == 'string') {
    try {
      let obj = JSON.parse(str);
      if (typeof obj == 'object' && obj) {
        return true;
      } else {
        return false;
      }
    } catch (e) {
      return false;
    }
  }
};
let callbackObject = {
  _a: null,
  _b: null,
};
export default class SignalR {
  constructor(topic) {
    this.config = config;
    this.connectionUrl = config.signalR.url;
    this.connection = HubConnection;
    this.topic = topic;
    this.registerMethod = "register";
  }
  getAccessToken() {
    let tokenKey = this.config.tokenKey;
    let accessToken = localStorage.getItem(tokenKey);
    return accessToken;
  }
  getConnectionUrl() {
    return `${this.connectionUrl}/${this.topic}`;
  }
  handleStartConnection() {
    if (this.connection) {
      const builder = new HubConnectionBuilder();
      const accessToken = this.getAccessToken();
      this.connection = builder
        .withUrl(this.getConnectionUrl(), {
          accessTokenFactory: () => accessToken,
          transport: HttpTransportType.LongPolling,
        }).build();
      this.connection.on(this.registerMethod, this.registerCallback);
    }
    if (this.connection.state !== HubConnectionState.Connected) {
      this.connection.start().then(() => this.connectionSuccessStart()).catch(() => this.connectionErrorStart());
    }
  }
  connectionSuccessStart() {
    this.registerDefault();
  }
  registerCallback(a, b, c, d, e, f, g) {
    let argumentsArr = [];
    Array.from(arguments).forEach(a => {
      checkJsonString(a) ? argumentsArr.push(JSON.parse(a)) : argumentsArr.push(a);
    });
    callbackObject.registerResult = argumentsArr;
  }
  registerDefault() {
    return new Promise((res, err) => {
      if (this.connection) {
        this.connection.invoke(this.registerMethod, this.topic)
          .then(res).catch(err);
      }
    });
  }
  checkLoginState() {
    //while (this.getAccessToken() == null) {}
    return true;
  }
  resolveReceiveData(a, b, c, d, e, f, g) {
    let argumentsArr = [];
    Array.from(arguments).forEach(a => {
      checkJsonString(a) ? argumentsArr.push(JSON.parse(a)) : argumentsArr.push(a);
    });
    callbackObject.realResult = argumentsArr;
  }
  init(callback) {
    Object.defineProperty(callbackObject, "registerResult", {
      set: function (value) {
        callback(...value);
      },
      get:function(){
        return callbackObject._a;
      },
      configurable: true,
    });
    if (this.checkLoginState()) {
      this.handleStartConnection();
    }
    return this;
  }
  on(method, callback) {
    Object.defineProperty(callbackObject, "realResult", {
      set: (newv) => {
        callback(...newv);
      },
      configurable: true,
    });
    if (this.connection) {
      this.connection.on(method, this.resolveReceiveData);
    }
    return this;
  }
  send(method, data) {
    if (this.connection) {
      this.connection.invoke(method, data).then(() => {}).catch(() => {});
    }
  }
}

// const signalr = new SignalR(config);
//export default signalr;
