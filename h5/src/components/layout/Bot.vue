<template>
<div id="bot">
    淮阴工学院Blog Copyright © TitanChen
    <el-badge :value="mssgCount" class="message" type="primary">
        <el-button type="primary" icon="el-icon-message" @click="openSendDialog" style="height:33px;"></el-button>
    </el-badge>
    <Send :visible.sync="sendVisible" :receiveid="testid" :receivename="testname" :PropMessages="Messages" />
</div>
</template>

<script>
import Send from '../chat/send'

export default {
    data() {
        return {
            mssgCount: 0,
            sendVisible: false,
            testid: 'ddd',
            testname: '淮安市三轮车',
            Messages: [],
        }
    },
    mounted: function () {
        var limit = JSON.parse(localStorage.getItem(_const.Key_CommonConfig)) || { Url: ''};
        var url = limit.Url === '' ? wy.getBaseUrl() : limit.Url + 'chatHub';
        this.openConnect(url);
    },
    components: {
        Send,
    },
    computed: {},
    methods: {
        openConnect: function (url) {
            this.$signalr.openConnect(url);
            this.receiveMessage();
        },
        receiveMessage: function(){
            this.$signalr.getConnect().on("ReceiveMessage", function (useraccount, username, message) {
                if (this.$store.state.user.UserAccount === useraccount) {
                    this.Messages.push({
                        senduser: username,
                        avatar: '',
                        content: message,
                        source: 'send',
                        time: wy.GetLongTimeString(new Date())
                    });
                } else {
                    this.Messages.push({
                        senduser: username,
                        avatar: '',
                        content: message,
                        source: 'receive',
                        time: wy.GetLongTimeString(new Date())
                    });
                    if (!this.sendVisible) {
                        this.mssgCount = this.mssgCount + 1;
                    }
                }
            });
        },
        userReceiveMessage: function(){
            this.$signalr.getConnect().on("UserReceiveMessage", function (useraccount, username, message) {
                var title = 'News From ' + username + '[' + useraccount + ']';
                var notify = {
                    title: title,
                    message: message,
                    position: 'bottom-right',
                    duration: 0,
                    type: 'success'
                }
                this.$notify(notify);
            });
        },
        openSendDialog: function () {
            this.sendVisible = true
            this.mssgCount = 0;
        }
    }
}
</script>

<style>
#bot {
    background-color: rgb(47, 73, 98);
    text-align: center;
    color: #bbbbbb;
    font-size: 12px;

    height: 30px;
    line-height: 30px;
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    z-index: 999;
}

.message {
    float: right;
    margin-right: 15px;
}
</style>
