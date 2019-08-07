<template>
    <div id="wy_content" class="wy-content">
        <el-card :body-style="{ padding: '10px 30px' }">
            <div class="wy-content-main" v-bind:style="{ height: mainHeight }">
                <slot></slot>
            </div>
        </el-card>
    </div>
</template>

<script>
export default {
    name: "WyContent",
    props: {
        
    },
    data() {
        return{
            IsQueryShow: true,
            IsMoreQueryShow: false,
            pageIndex: 1,
            pageSize: 20,
            mainHeight: '',
            mainWidth: '',
        }
    },
    created() {
        
    },
    mounted() {
        this.initialContentSize();
        window.onresize = () => {
            this.initialContentSize();
        };
    },
    methods: {
        initialContentSize: function(){
            var height =  window.innerHeight - 1;//可见区域高度包括滚动条
            this.mainHeight = height - 210 + 'px';
        },
    }
}
</script>
<style>
</style>
