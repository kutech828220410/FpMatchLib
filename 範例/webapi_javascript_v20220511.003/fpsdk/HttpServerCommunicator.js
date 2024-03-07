/**
 *
 * use http
 */
var HttpServerCommunicator = {
    returnCallback:null,
    closeCallback:null,
    errorCallback:null,
    curCmd : '',

    setPort:function(port){
        this.__port = port;
    },
    connect:function (cmdObj, openCallback, errorCallback){

        this.curCmd = cmdObj.cmd

        $.ajax({
            url: this.__getHost() + JSON.stringify(cmdObj),
            type: "GET",
            dataType: "jsonp",
            cache: false,
            success: function (data) {
                if( openCallback ){
                    openCallback(data);
                }

            },error: function (jqXHR, textStatus, errorThrown) {
                if(errorCallback){
                    errorCallback(jqXHR, textStatus, errorThrown);
                }
            }
        });

    },

    talk:function (cmdObj, returnCallback, errorCallback){

        this.curCmd = cmdObj.cmd
        this.returnCallback = returnCallback;
        this.errorCallback = errorCallback;

        $.ajax({
            url: this.__getHost() + JSON.stringify(cmdObj),
            type: "GET",
            dataType: "jsonp",
            cache: false,
            success: function (data) {
                console.log(`url [${arguments.callee.name}]` , url);
                console.log(`data [${arguments.callee.name}]` , data);
                if( returnCallback ){
                    returnCallback(data);
                   
                }

            },error: function (jqXHR, textStatus, errorThrown) {
                if(errorCallback){
                    errorCallback(jqXHR, textStatus, errorThrown);
                }
            }
        });

    },

    __port: 19002, // 默认端口

    __getHost:function (){
        return "http://127.0.0.1:" + this.__port + "/http/";
    }

}