<template>
<div id="app">
    <!-- 路由匹配到的组件将渲染在这里 -->
    <keep-alive>
        <router-view v-if="$route.meta.keepAlive"></router-view>
    </keep-alive>
    <router-view v-if="!$route.meta.keepAlive"></router-view>
</div>
</template>

<script>
export default {
    mounted: function () {},
    methods: {}
}
</script>
