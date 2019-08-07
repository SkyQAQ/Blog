<template>
<div id="mid">
    <!-- 路由匹配到的组件将渲染在这里 -->
    <keep-alive>
        <router-view v-if="$route.meta.keepAlive"></router-view>
    </keep-alive>
    <router-view v-if="!$route.meta.keepAlive"></router-view>
</div>
</template>

<script>
export default {
    data() {
        return {}
    },
}
</script>

<style>
#mid {
    padding-top: 90px;
    padding-bottom: 26px;
    padding-left: 60px;
    padding-right: 60px;
}
</style>