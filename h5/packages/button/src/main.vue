<template>
    <div id="wy_btton">
        <slot></slot>
        <template v-if="showBack || showAutoback">
            <el-button type="text" @click="handleBackClick">返回</el-button>
        </template>
    </div>
</template>

<script>
export default {
    name: "WyButton",
    props: {
        showBack: {
            type: Boolean,
            default: false,
            required: false
        },
        showAutoback: {
            type: Boolean,
            default: false,
            required: false
        }
    },
    methods: {
        handleBackClick() {
            if (this.showAutoback) {
                if(!this.$route.params.id){
                    this.$confirm('单据还未保存, 是否继续返回 ?', '提示', {
                        confirmButtonText: '确认',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        this.$router.back();
                    }).catch(() => {});
                }else{
                    this.$router.back();
                }
            } else {
                this.$emit("goback");
            }
        }
    }
}
</script>