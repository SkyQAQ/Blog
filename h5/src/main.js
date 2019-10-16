import Vue from 'vue';
import router from './router';
import store from './store';

import './assets/js/blog.js';
import './assets/css/blog.css';

// Element组件引用
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
Vue.use(ElementUI);
//自定义组件
import BlogUI from '../packages';
Vue.use(BlogUI);

// HTTP请求plugin
import Http from "@/plugins/http";
Vue.use(Http);
// 身份验证plugin
import Auth from "@/plugins/auth";
Vue.use(Auth);
// 身份验证plugin
import Signalr from "@/plugins/signalr";
Vue.use(Signalr);
// 常量plugin
import Constant from "@/plugins/constant";
Vue.use(Constant);

// 开启debug模式
Vue.config.debug = true;
// 正式环境不提示
Vue.config.productionTip = false
    // 配置是否允许vue-devtools 检查代码  默认为true
Vue.config.devtools = true;

// 视频播放器
import VideoPlayer from 'vue-video-player'
require('video.js/dist/video-js.css')
require('vue-video-player/src/custom-theme.css')
Vue.use(VideoPlayer)

import App from './App';
// 全局构造函数
new Vue({
    el: '#app',
    router,
    store,
    template: '<App/>',
    components: {
        App
    }
});