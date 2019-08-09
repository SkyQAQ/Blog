<template>
<div id="top">
    <div class="title">
        <div style="float:right;margin-right:60px">
            <el-dropdown trigger="click" @command="handleCommand">
                <span class="el-dropdown-link">
                    <img alt="" :src="avatarUrl" class="round-icon"/>
                </span>
                <el-dropdown-menu slot="dropdown">
                    <el-dropdown-item command="changeinfo">个人信息</el-dropdown-item>
                    <el-dropdown-item command="chageavatar">更换头像</el-dropdown-item>
                    <el-dropdown-item command="logout">注销</el-dropdown-item>
                </el-dropdown-menu>
            </el-dropdown>
        </div>
    </div>
    <div class="menu">
        <Menu></Menu>
    </div>
    <UserInfo :visible.sync="userinfoVisible" v-if='userinfoVisible' />
    <UserAvatar ref="useravatar" @loadAvatar="loadAvatar" />
</div>
</template>

<script>
import Menu from './Menu';
import defaultAvatar from "../../assets/imgs/avatar.png";
import UserInfo from "../account/userinfo.vue";
import UserAvatar from "../account/useravatar.vue";

export default {
    data() {
        return {
            avatarUrl: '',
            userinfoVisible: false,
            avatarVisible: false,
            uploadUrl: '',
        }
    },
    mounted() {
        this.onLoad();
    },
    components: {
        Menu,
        UserInfo,
        UserAvatar,
    },
    methods: {
        onLoad: function () {
            var limit = JSON.parse(localStorage.getItem(_const.Key_CommonConfig)) || { Url: ''};
            if (this.$store.state.user.UserAvatar) {
                this.avatarUrl = limit.Url === '' ? wy.getBaseUrl() + 'api/userinfo/changeavatar' : limit.Url + 'api/auth/avatar?id=' + this.$store.state.user.UserAvatar;
            }
            else{
                this.avatarUrl = defaultAvatar;
            }
            this.uploadUrl = limit.Url === '' ? wy.getBaseUrl() + 'api/userinfo/changeavatar' : limit.Url + 'api/userinfo/changeavatar';
        },
        loadAvatar: function(avatar){
            this.avatarUrl = avatar;
        },
        logout: function () {
            var apiUrl = 'api/auth/logout?userId=' + this.$store.state.user.UserId;
            wy.get(apiUrl).then((res) => {
                this.$signalr.closeConnect();
                this.$auth.clearToken();
            }).catch((error) => {
                wy.showErrorMssg(error);
            });
        },
        handleCommand: function (command) {
            if (command === 'changeinfo') {
                this.userinfoVisible = true;
            } else if (command === 'chageavatar') {
                this.$refs.useravatar.show(this.uploadUrl);
            } else if (command === 'logout') {
                this.logout();
            }
        }
    }
}
</script>

<style scoped>
.title {
    background-color: rgb(47, 73, 98);
    text-align: center;
    color: #bbbbbb;
    font-size: 12px;

    height: 40px;
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    z-index: 999;
}

.menu {
    background-color: #3B5B7B;
    top: 40px;
    left: 0;
    right: 0;
    height: 50px;
    padding-left: 80px;
    position: fixed;
    z-index: 999;
}
</style>
