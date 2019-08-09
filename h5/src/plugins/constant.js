window.key = {};
export default {
    install(Vue, options) {
        this.onLoadConst();
    },
    bindToGlobal: function(obj, key = '_const') {
        if (typeof window[key] === 'undefined') {
            window[key] = {};
        }
        for (let i in obj) {
            window[key][i] = obj[i]
        }
    },
    onLoadConst: function() {
        var obj = {
            JobDelete: 'delete',
            JobPause: 'pause',
            JobResume: 'resume',
            JobStop: 'stop',
            JobStart: 'start',
            JobExcute: 'excute',
            ReceiveTypeEmail: 'Email',
            ReceiveTypePhone: 'Phone',
            CodeTypeRegister: 'Register',
            CodeTypeForgetPwd: 'ForgetPwd',
            CodeTypeChangeEmail: 'ChangeEmail',
            Key_AccessRoute: 'access_route',
            Key_CommonConfig: 'common_config',
        };
        this.bindToGlobal(obj);
    }
}