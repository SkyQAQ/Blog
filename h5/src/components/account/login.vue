<template>
<div id="div_login" class="div_login" :style="{ width: windowWidth + 'px', height: windowHeight + 'px' }">
    <div id="div_form" class="div_form" :style="{ 'margin-top': loginDivTop + 'px', 'margin-left': loginDivLeft + 'px' }">
        <p class="form_title">淮安三轮车开黑网站</p>
        <div class="div_input" @keyup.enter="login">
            <el-form ref="loginform" :rules="rules" :model="identity">
                <el-form-item prop="username">
                    <el-input placeholder="账号/邮箱" type="text" v-model="identity.username" auto-complete="off" prefix-icon="el-icon-cust-account">
                    </el-input>
                </el-form-item>
                <el-form-item prop="password">
                    <el-input placeholder="密码" type="password" v-model="identity.password" auto-complete="off" prefix-icon="el-icon-cust-password">
                    </el-input>
                </el-form-item>
                <el-form-item prop="verifycode">
                    <el-input placeholder="验证码" type="text" v-model="identity.verifycode" auto-complete="off" prefix-icon="el-icon-cust-captch">
                        <template slot="append" style="border:none;"><img alt="" title="点击刷新" :src="this.VerifyCodeImageSrc" @click="getVerifyCode"/></template>
                    </el-input>
                </el-form-item>
                <el-form-item prop="btnlogin">
                    <el-button @click.native.prevent="login" style="width:400px" :loading="loading">登录</el-button>
                </el-form-item>
                <el-button type="text" @click="registerVisible = true">注册</el-button>
                <el-button type="text" @click="forgetpwdVisible = true">忘记密码?</el-button>
            </el-form>
        </div>
    </div>
    <Register :visible.sync="registerVisible" />
    <ForgetPwd :visible.sync="forgetpwdVisible" />
</div>
</template>

<script>
// AES DES MD5...加密
// import Crypto from 'crypto-js'
import RsaHelper from 'jsencrypt'
import Register from './register.vue'
import ForgetPwd from './forgetpwd.vue'

export default {
    data() {
        return {
            VerifyCodeImageSrc: '',
            identity: {
                username: '',
                password: '',
                verifycode: '',
            },
            captch: {
                verifyCodeString: '',
                verifyCodeBaseString: '',
            },
            rules: {
                username: [{
                    required: true,
                    message: '请输入账号',
                    trigger: 'blur'
                }],
                password: [{
                        required: true,
                        message: '请输入密码',
                        trigger: 'blur'
                    },
                    // { min: 8, max: 16, message: '长度在 8 到 16 个字符', trigger: 'blur' }
                ],
                verifycode: [{
                    required: true,
                    message: '请输入验证码',
                    trigger: 'blur'
                }]
            },
            registerVisible: false,
            forgetpwdVisible: false,
            loading: false,
        }
    },
    mounted: function () {
        this.getVerifyCode();
    },
    components: {
        Register,
        ForgetPwd,
    },
    computed: {
        windowHeight: function () {
            return document.documentElement.clientHeight || document.body.clientHeight || window.innerHeight;
        },
        windowWidth: function () {
            return document.documentElement.clientWidth || document.body.clientWidth || window.innerWidth;
        },
        loginDivTop: function () {
            return (this.windowHeight - 400) / 2;
        },
        loginDivLeft: function () {
            return (this.windowWidth - 600) / 2;
        }
    },
    methods: {
        getVerifyCode: function () {
            wy.get('api/auth/verifycode').then((res) => {
                this.captch = res;
                this.VerifyCodeImageSrc = 'data:image/png;base64,' + res.VerifyCodeBaseString;
            }).catch((error) => {
                wy.showErrorMssg(error);
            });
        },
        login: function () {
            this.$refs['loginform'].validate((valid) => {
                if (valid && !this.loading) {
                    let credit = {
                        grant_type: 'password',
                        username: this.identity.username,
                        password: wy.rsa(this.identity.password),
                        verifycode1: this.identity.verifycode,
                        verifycode2: encodeURIComponent(this.captch.VerifyCodeString),
                        mac: '',
                    }
                    this.accessToken(credit);
                }
            });
        },
        accessToken: function (credit) {
            this.loading = true;
            let data = 'grant_type=password' + '&username=' + credit.username + '&password=' + credit.password + '&verifycode1=' + credit.verifycode1 + '&verifycode2=' + credit.verifycode2 + '&mac=' + credit.mac;
            wy.post('api/auth/tokenjwt', data).then((res) => {
                this.$auth.storeToken(res);
                this.getUserInfo();
                this.getUploadLimit();
                this.getAccessRoute();
            }).catch((error) => {
                this.loading = false;
                this.getVerifyCode();
                wy.showErrorMssg(error);
            });
        },
        getUserInfo: function () {
            wy.get('api/userinfo/getuserinfo').then((res) => {
                this.loading = false;
                this.$auth.storeUser(res);
                this.$router.push({
                    name: "home",
                });
                wy.showSuccessMssg('登录成功');
            }).catch((error) => {
                this.loading = false;
                wy.showErrorMssg(error);
            });
        },
        getAccessRoute: function() {
            wy.get('api/menuinfo/accessroute').then((res) => {
                localStorage.setItem(_const.Key_AccessRoute, JSON.stringify(res));
            }).catch((error) => {
                wy.showErrorMssg(error);
            });
        },
        getUploadLimit: function() {
            wy.get('api/attachment/uploadlimit').then((res) => {
                localStorage.setItem(_const.Key_UploadLimit, JSON.stringify(res));
            }).catch((error) => {
                wy.showErrorMssg(error);
            });
        }
    }
}
</script>

<style scoped>
.div_login {
    background: url('~@/assets/imgs/login_bg.jpg') no-repeat;
    background-size: 100% 100%;
    padding: 0;
    top: 0px;
    left: 0px;
    position: absolute;
}

.div_form {
    width: 600px;
    height: 400px;
    border: 1px rgb(167, 204, 221);
    border-radius: 5px;
    background-color: rgba(255, 255, 255, 0.5);
    text-align: center;
}

.div_input {
    width: 400px;
    height: 300px;
    position: absolute;
    left: 50%;
    margin-top: 10px;
    margin-left: -200px;
}

.form_title {
    padding-top: 50px;
    font-size: 24px;
    color: snow;
    font-family: 'Segoe UI',
}
</style><style>
.el-input--prefix .el-input__inner {
    padding-left: 50px;
}
</style>
