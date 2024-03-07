/**
 *
 * use websocket
 */
var WsServerCommunicator = {
    returnCallback:null,
    closeCallback:null,
    errorCallback:null,
    curCmd : '',

    setPort:function(port){
        this.__port = port;
    },

    connect:function (cmdObj, returnCallback, errorCallback, closeCallback){
        console.log(`cmdObj [${arguments.callee.name}]` , cmdObj);
        this.returnCallback = returnCallback;
        this.closeCallback = closeCallback;
        this.errorCallback = errorCallback;

        this.__ws = new WebSocket(this.__getHost() );
        this.__ws.onopen = function (){
            if(This.returnCallback){
                This.returnCallback();
            }
        }

        This = this
        this.__ws.onmessage = function(e){
            /**
             当客户端收到服务端发来的消息时，触发onmessage事件，参数e.data包含server传递过来的数据//
             */
             console.log(This.curCmd + " return:" + e.data);
            if(This.returnCallback){
                var retObj = JSON.parse(e.data)
                Fp.t = retObj.t;
                This.returnCallback(retObj);
            }

        }

        this.__ws.onclose = function(e){
            /**
             当客户端收到服务端发送的关闭连接请求时，触发onclose事件
             */
            console.log(This.curCmd + " connect is closed.");
            if(This.closeCallback){
                This.closeCallback(e);
            }
        }

        this.__ws.onerror = function(e){
            /**
             如果出现连接、处理、接收、发送数据失败的时候触发onerror事件
             */
            console.log(This.curCmd + " websocket error:" + e);
            if(This.errorCallback){
                This.errorCallback(e);
            }

        }

    },

    talk:function (cmdObj, returnCallback, errorCallback, closeCallback){

        this.curCmd = cmdObj.cmd
        this.returnCallback = returnCallback;
        this.closeCallback = closeCallback;
        this.errorCallback = errorCallback;
        console.log(`cmdObj [${arguments.callee.name}]` , cmdObj);
        // console.log("ws.readyState:" + this.__ws.readyState)

        if( this.__ws.readyState == 1){
            this.__ws.send(JSON.stringify(cmdObj)); //将消息发送到服务端
        }else{
            This = this
            this.__ws.onopen = function (){
                This.__ws.send(JSON.stringify(cmdObj)); //将消息发送到服务端
            }
        }

    },

    __port: 19002, // 默认端口

    __getHost:function (){
        console.log("ws://127.0.0.1:" + this.__port + "/ws");
        return "ws://127.0.0.1:" + this.__port + "/ws";
    }

}