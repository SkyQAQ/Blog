<template>
<div id="chat_send">
    <el-dialog :title="receivename" :visible.sync="visible" :close-on-click-modal="false" :before-close="beforeClose" :modal="false" :lock-scroll="false" width="40%" :open="updateScroll">
        <div id="div_message" class="div_message">
            <ul class="mssg_list">
                <li v-for="mssg in PropMessages" :key="mssg.index" :class="mssg.source==='receive'?'li_receive':'li_send'">
                    <img alt="" src="~@/assets/logo.png" width="30px" v-if="mssg.source==='receive'"/>
                    <span style="vertical-align: super;font-size: 12px;color: gray;" v-if="mssg.source==='receive'">{{mssg.senduser}}</span>&nbsp;
                    <span style="vertical-align: super;font-size: 12px;color: gray;">{{mssg.time}}</span>
                    <img alt="" src="~@/assets/logo.png" width="30px" v-if="mssg.source==='send'"/>
                    <br>
                    <span>{{mssg.content}}</span>
                </li>
            </ul>
        </div>
        <div class="div_operate" @keyup.ctrl.enter="send">
            <el-input ref="textarea_mssg" type="textarea" :rows="3" placeholder="消息内容 按Ctrl+Enter发送" :autosize="{ minRows: 3, maxRows: 3}" v-model="sendMessage" resize="none"></el-input>
            <div class="operate_button">
                <el-button size="mini" @click="beforeClose">关闭</el-button>
                <el-button type="primary" size="mini" @click="send" spellcheck="false">发送</el-button>
            </div>
        </div>
        <span slot="footer"></span>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        return {
            sendMessage: '',
            loading: false,
            sendButtonDisabled: false,
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
        receiveid: {
            type: String,
            default: '',
            require: true
        },
        receivename: {
            type: String,
            default: '',
            require: true
        },
        PropMessages: {
            type: Array,
        }
    },
    mounted: function () {

    },
    computed: {

    },
    updated: function () {
        this.updateScroll();
    },
    watch: {},
    methods: {
        beforeClose: function () {
            this.$emit('update:visible', false);
        },
        send: function () {
            if (this.sendMessage) {
                this.$signalr.sendMessage(this.sendMessage);
                this.sendMessage = '';
                this.$refs.textarea_mssg.focus();
            } else {
                alert('不能输入空消息')
            }
        },
        updateScroll: function () {
            this.$nextTick(function () {
                var mssgDiv = document.getElementById('div_message');
                if (mssgDiv.scrollHeight != null) {
                    mssgDiv.scrollTop = mssgDiv.scrollHeight;
                }
            });
        }
    }
}
</script>

<style>
.div_message {
    margin-left: 20px;
    margin-right: 20px;
    height: 45%;
    border: 1px solid rgb(190, 190, 190);
    overflow-y: auto;
}

.div_message::-webkit-scrollbar {
    /*滚动条整体样式*/
    width: 5px;
    /*高宽分别对应横竖滚动条的尺寸*/
    height: 1px;
}

.div_message::-webkit-scrollbar-thumb {
    /*滚动条里面小方块*/
    border-radius: 10px;
    background: rgb(190, 190, 190);
}

.div_message::-webkit-scrollbar-track {
    /*滚动条里面轨道*/
    background: rgb(255, 255, 255);
}

.div_operate {
    margin-left: 20px;
    margin-right: 20px;
    height: 100px;
    border: 1px solid rgb(190, 190, 190);
    border-top: none;
}

.operate_button {
    float: right;
    padding-right: 5px;
}

.operate_button button {
    width: 100px;
}

.mssg_list {
    margin-left: -30px;
    margin-right: 10px;
}

.mssg_list li {
    list-style: none;
    width: 100%;
}

.li_receive {
    text-align: left;
}

.li_send {
    text-align: right;
}
</style>
