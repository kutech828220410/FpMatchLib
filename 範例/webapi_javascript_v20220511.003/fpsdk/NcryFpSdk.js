/**
 *   纳彩瑞远科技 - 指纹开发包 WebApi v2.0.1
 *      
 *   
 */
var Fp = {
    // 访问指纹设备函数的返回值
    RT_SUCCESS                          :0x00,
    RT_FAIL                             :0x01,
    RT_DEVICE_NOT_FOUND					:0x06,
    RT_DEVICE_BUSY						:0x07,
    RT_BAD_QUALITY						:0x21,
    RT_GENERALIZE						:0x30,
    RT_FP_CANCEL						:0x41,

    RT_NEED_FIRST_SWEEP					:0xFFF1,
    RT_NEED_SECOND_SWEEP	   			:0xFFF2,
    RT_NEED_THIRD_SWEEP					:0xFFF3,
    RT_NEED_RELEASE_FINGER	 			:0xFFF4,

    MAX_IMAGE_SIZE				        :75000,
    FP_TEMPLATE_SIZE					:768,

    SCSI_DEVICE :0,
    SERIAL_DEVICE: 1,

    // 设置指纹服务的端口，一般不用改
    setPort:function(port){
        this.__getServerCommunicator().setPort(port);
    },

    /**
     * 连接指纹服务
     * @param {callback} openCallback 连接成功的回调函数
     * @param {callback} errorCallback 连接失败的回调函数
     */
     connect:function (openCallback, errorCallback){
        var cmdObj = {
            cmd:this.CmdType.Connect
        };
        this.__getServerCommunicator().connect(cmdObj, openCallback, errorCallback);
    },

    /**
     * 设置开发包与指纹设备的通讯类型。
     * @param {int} deviceType 设备类型，取值0或者1，默认0。其中0表示：usb-scsi；1表示：usb-串口
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     * retCode: 取值RT_SUCCESS，表示设置设备类型成功
     */
    setDeviceType:function (deviceType, returnCallback){
        var cmdObj = {
            cmd:this.CmdType.SetDeviceType,
            deviceType: deviceType
        };

        this.talk(cmdObj, returnCallback);
    },

    /**
     * 返回当前开发包与指纹设备的通讯类型
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     *  retCode: 取值RT_SUCCESS，表示设置设备类型成功；
     *  deviceType:设备类型，取值0或者1。其中0表示：usb-scsi；1表示：usb-串口；
     */
    getDeviceType:function (returnCallback){
        var cmdObj = {cmd:this.CmdType.GetDeviceType};
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 获取当前接入电脑的指纹设备数量
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     * count: 设备数量
     * 
     */
    deviceCount:function (returnCallback){
        var cmdObj = {cmd:this.CmdType.DeviceCount};
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 关闭指纹设备
     * @param {int} deviceIndex 插入电脑的指纹设备序号，从0开始。当取值-1时，表示忽略序号，打开第一个可用的指纹设备。
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有： 
     * handle: 设备句柄，用于后续访问该设备，如果handle小于或者等于0，则表示打开指纹设备失败。此时考虑设备是否插入，设备驱动是否安装。
     */
    openDevice:function (deviceIndex, returnCallback){
        var cmdObj = {cmd:this.CmdType.OpenDevice,
            deviceIndex:deviceIndex};
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 关闭指纹设备
     * @param {int} handle handle 设备句柄，由openDevice返回
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     * retCode: 取值RT_SUCCESS, RT_FAIL 分别表示设置设备类型成功或失败
     */
    closeDevice:function (handle, returnCallback){
        var cmdObj = {
            handle:handle,
            cmd:this.CmdType.CloseDevice
        };
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 登记指纹，按三次手指完成一次指纹登记
     * @param {int} handle 设备句柄
     * @param {callback} returnCallback  回调函数，参数为json对象，包含字段有：
     * stateCode:返回的状态码，取值如下：
     *  -1: 默认返回值，不需处理
     *   RT_SUCCESS：登记指纹成功，此时feature和featureLen的值有效
     *   RT_NEED_FIRST_SWEEP：请用户按第一次手指
     *   RT_NEED_SECOND_SWEEP：请用户按第二次手指
     *   RT_NEED_THIRD_SWEEP：请用户按第三次手指
     *   RT_NEED_RELEASE_FINGER：请用户移开手指
     *   RT_BAD_QUALITY：指纹质量低，需继续采集
     *   RT_FP_CANCEL：当前任务被取消，此时应该退出循环体，让出指纹设备
     * feature: 指纹特征模板，只有在stateCode为RT_SUCCESS时有值
     * featureLen:指纹模板数据长度，只有在stateCode为RT_SUCCESS时有值
     * 
     */
    enroll:function (handle, returnCallback){
        var cmdObj = {
            handle: handle,
            cmd:this.CmdType.Enroll
        };
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 实时获取用户指纹特征
     * @param {int} handle 设备句柄
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     * stateCode:返回的状态码，取值如下：
     *  -1: 默认返回值，不需处理
     *   RT_SUCCESS：指纹特征获取成功，此时feature和featureLen的值有效
     *   RT_BAD_QUALITY：指纹质量低，需继续采集
     *   RT_FP_CANCEL：当前任务被取消，此时应该退出循环体，让出指纹设备
     * feature: 指纹特征数据，只有在stateCode为RT_SUCCESS时有值
     * featureLen:指纹特征数据长度，只有在stateCode为RT_SUCCESS时有值
     */
    getFeature:function (handle, returnCallback){
        var cmdObj = {
            handle: handle,
            cmd:this.CmdType.GetFeature
        };
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 实时获取用户指纹图像，返回位图格式图像，为方便传输该位图已做base64编码。
     * @param {int}  handle 设备句柄
     * @param {callback}  returnCallback 回调函数，参数为json对象，包含字段有：
     * stateCode:返回的状态码，取值如下：
     *  -1: 默认返回值，不需处理
     *   RT_SUCCESS：指纹图像获取成功，此时image和imageLen的值有效
     *   RT_BAD_QUALITY：指纹质量低，需继续采集
     *   RT_FP_CANCEL：当前任务被取消，此时应该退出循环体，让出指纹设备
     * image: 指纹图像数据（base64编码），只有在stateCode为RT_SUCCESS时有值。保存到文件前，先base64解码，然后保存到文件，文件后缀为.bmp，
     * 即可得到位图文件。
     * imageLen:指纹图像数据（base64编码）长度，只有在stateCode为RT_SUCCESS时有值
     */
    getImage:function (handle, returnCallback){
        var cmdObj = {
            handle: handle,
            cmd:this.CmdType.GetImage
        };
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 设置指纹设备的参数
     * @param {int} handle 设备句柄
     * @param {int} parameterId 参数id，目前的参数有
     * CNF_SECLEVEL：设备安全级别的设置
     * 
     * @param {int} value 参数对应的值
     * CNF_SECLEVEL：设备安全级别的值，默认为3，取值范围3、4、5
     * 
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     * retCode: 取值RT_SUCCESS，表示设置设备参数成功
     */
    setParameter:function (handle, parameterId, value, returnCallback){
        var cmdObj = {
            handle: handle,
            cmd:this.CmdType.SetParameter,
            parameterId: parameterId,
            value: value
        };
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 获取指纹设备的参数
     * @param {int} handle 设备句柄
     * @param {int} parameterId 参数id，目前的参数有
     * CNF_SECLEVEL：设备安全级别的设置
     * 
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     * retCode: 取值RT_SUCCESS，表示设置设备参数成功
     */
    getParameter:function (handle, parameterId, returnCallback){
        var cmdObj = {
            handle: handle,
            cmd:this.CmdType.GetParameter,
            parameterId: parameterId
        };
        this.talk(cmdObj, returnCallback);
    },

    /**
     * 获取指纹设备编号
     * @param {int} handle 设备句柄
     * @param {callback} returnCallback 回调函数，参数为json对象，包含字段有：
     * retCode: 取值RT_SUCCESS，表示设置设备参数成功
     * sn: 设备编号
     */
    getDeviceSn:function(handle, returnCallback){
        var cmdObj = {
            handle: handle,
            cmd:this.CmdType.GetDeviceSn
        }

        this.talk(cmdObj, returnCallback);
    },


    /**
     * 辅助函数，在一个id为console的元素里打印消息。
     * @param {string} msg 
     */
    log:function (msg){
        var txt = "<div>" + msg + "</div>"
        var consoleEl = document.getElementById('console')
        if( consoleEl ){
            consoleEl.innerHTML += (txt);
        }
    },

    /**
     * 辅助函数，模拟休眠函数
     * @param {long} milliSecond 
     * @returns void
     */
    sleep:function(milliSecond){
        var timeStamp = new Date().getTime();
        var endTime = timeStamp + milliSecond;
        while(true){
            if (new Date().getTime() > endTime){
                return;
            }
        }
    },


    returnCallback:null,
    closeCallback:null,
    errorCallback:null,
    curCmd : '',



    // 开发包内部函数，不能使用
    talk:function (cmdObj, returnCallback, errorCallback){
        this.__getServerCommunicator().talk(cmdObj, returnCallback, errorCallback);
    },



    CmdType : {
        Connect:0,
        SetDeviceType: 1,
        GetDeviceType: 2,
        DeviceCount: 3,
        OpenDevice: 4,
        CloseDevice: 5,
        Enroll: 6,
        GetFeature: 7,
        SetParameter: 8,
        GetParameter: 9,
        GetDeviceSn: 10,
        CodeToStr: 11,
        GetImage:12
    },

    __serverCommunicator:null,

    /**
     * 开发包内部函数，不能使用
     * @returns {null}
     * @private
     */
    __getServerCommunicator:function (){
        if (this.__serverCommunicator == null){
            if( window.WebSocket ){
                this.log("@@ using websocket version");
                this.__serverCommunicator = WsServerCommunicator;
            }else{
                this.log("@@ using http version");
                this.__serverCommunicator = HttpServerCommunicator;
            }
        }

        return this.__serverCommunicator;
    }

}


