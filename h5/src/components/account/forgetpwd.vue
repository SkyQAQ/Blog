<template>
<div id="forgetpwd">
    <el-dialog title="忘记密码" :visible.sync="visible" :before-close="beforeClose" :close-on-click-modal="false" width="50%">
        <div class="div_input">
            <el-form ref="resetpwdform" :rules="rules" :model="model">
                <el-form-item prop="receive">
                    <el-input placeholder="邮箱" type="text" v-model="model.receive" auto-complete="off" prefix-icon="el-icon-message">
                        <el-button slot="append" @click="sendVerifyCode" :disabled="sendButtonDisabled" :loading="loading1">{{ sendButtonText }}</el-button>
                    </el-input>
                </el-form-item>
                <el-form-item prop="verifycode">
                    <el-input placeholder="验证码" type="text" v-model="model.verifycode" auto-complete="off" prefix-icon="el-icon-view">
                    </el-input>
                </el-form-item>
                <el-form-item prop="btnresetpwd">
                    <el-button @click.native.prevent="resetpwd" :loading="loading" style="width:300px">重置密码</el-button>
                </el-form-item>
                <span class="span_mssg">{{ message }}</span>
            </el-form>
        </div>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        return {
            model: {
                receive: '',
                verifycode: ''
            },
            rules: {
                receive: [{
                    required: true,
                    message: '请输入邮箱',
                    trigger: 'blur'
                }],
                verifycode: [{
                    required: true,
                    message: '请输入验证码',
                    trigger: 'blur'
                }]
            },
            loading: false,
            loading1: false,
            sendButtonText: '发送验证码',
            sendButtonDisabled: false,
            message: '',
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
    },
    mounted: function () {

    },
    computed: {

    },
    methods: {
        beforeClose: function () {
            this.$emit('update:visible', false);
        },
        sendVerifyCode: function () {
            if (!this.model.receive) {
                wy.showErrorMssg('请输入电子邮箱！');
                return;
            }
            this.loading1 = true;
            var api = 'api/auth/receiveverifycode';
            var params = {
                receive: this.model.receive,
                codetype: _const.CodeTypeForgetPwd
            };
            wy.get(api, params).then((res) => {
                this.loading1 = false;
                this.message = res;
                this.disableSendBtn();
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading1 = false;
            });
        },
        disableSendBtn: function () {
            var that = this;
            var sencond = 60;
            that.sendButtonDisabled = true;
            that.sendButtonText = sencond + '秒后重试';
            var itv = setInterval(function () {
                sencond--;
                that.sendButtonText = sencond + '秒后重试';
                if (sencond < 1) {
                    clearInterval(itv);
                    that.sendButtonText = '发送验证码';
                    that.sendButtonDisabled = false;
                }
            }, 1000);
        },
        resetpwd: function () {
            this.$refs['resetpwdform'].validate((valid) => {
                if (valid) {
                    this.loading = true;
                    var api = 'api/auth/resetpwd';
                    var params = {
                        receive: this.model.receive,
                        verifycode: this.model.verifycode
                    };
                    wy.get(api, params).then((res) => {
                        this.message = res;
                        this.loading = false;
                    }).catch((error) => {
                        wy.showErrorMssg(error);
                        this.loading = false;
                    });
                }
            });
        }
    }
}
</script>

<style scoped>
.div_input {
    margin-left: 100px;
    margin-right: 100px;
}

.span_mssg {
    color: red;
    font-size: 12px;
}
</style>
