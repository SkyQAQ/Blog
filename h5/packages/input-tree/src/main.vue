<template>
   <div id="wy_input_tree" class="wy-input-tree" ref="inputtreediv">
        <el-input v-model="model.Name" @focus="istreeshow=true" @change="nameChange" :disabled="disabled" :clearable="clearable"></el-input>
        <div class="arrow" v-show="istreeshow"></div>
        <div class="treedata" v-show="istreeshow">
            <el-tree ref="inputtree" :data="treedata" :expand-on-click-node="false" :filter-node-method="filterNode" node-key="id" auto-expand-parent @node-click="handleClick" :highlight-current="true">
            </el-tree>
        </div>
   </div>
</template>

<script>
    export default {
        name: 'WyInputTree',
        data() {
            return {
                model: {
                    Id: '',
                    Name: '',
                },
                istreeshow: false,
            }
        },
        props: {
            treedata: {
                type: Array,
                required: true,
            },
            value: {
                type: Object,
                required: false,
                default: function(){
                    return { Id: '', Name: ''};
                }
            },
            disabled: {
                type: Boolean,
                required: false,
                default: false,
            },
            clearable: {
                type: Boolean,
                required: false,
                default: true,
            },
            disabledNode: {
                type: String,
                required: false,
                default: '',
            }
            // idFiled: {
            //     type: String,
            //     required: false
            // },
            // nameFiled: {
            //     type: String,
            //     required: true
            // },
        },
        watch: {
            value(val) {
                this.model = val;
            },
            'model.Name': function(val){
                this.$refs.inputtree.filter(val);
            }
        },
        //监控页面点击事件
        mounted() {
            this.model = this.value;
            let that = this;
            document.addEventListener('click', (e) => {
                if(that.$refs.inputtreediv === undefined){
                    return;
                }
                if (!that.$refs.inputtreediv.contains(e.target)) {
                    that.istreeshow = false;
                }
            });
        },
        methods: {
            filterNode: function(value, data) {
                if (!value) return true;
                return data.label.indexOf(value) !== -1;
            },
            handleClick: function(data){
                if(this.disabledNode === data.id){
                    wy.showErrorMssg('当前节点无法选用！');
                    return;
                }
                this.model.Id = data.id;
                this.model.Name = data.label;
            },
            nameChange: function(){
                if(!this.model.Name){
                    this.model.Id = '00000000-0000-0000-0000-000000000000';
                }
            }
        }
    }
</script>
<style>
    .wy-input-tree .treedata {
        min-height: 150px;
        max-height: 180px;
        width:100%;
        overflow: auto;
        position: absolute;
        top: 37px;
        padding: 5px 0 0;
        z-index: 999;
        border: 1px solid #e4e7ed;
        border-radius: 4px;
        background-color: #fff;
        box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
    }
    .wy-input-tree .treedata::-webkit-scrollbar {/*滚动条整体样式*/
        width: 5px;     /*高宽分别对应横竖滚动条的尺寸*/
        height: 1px;
    }
    .wy-input-tree .treedata::-webkit-scrollbar-thumb {/*滚动条里面小方块*/
        border-radius: 10px;
        background: rgb(190, 190, 190);
    }
    .wy-input-tree .treedata::-webkit-scrollbar-track {/*滚动条里面轨道*/
        background: rgb(255, 255, 255);
    }
    .wy-input-tree .arrow {
        content: " ";
        position: absolute;
        left: 21%;
        top: 32px;
        width: 10px;
        height: 10px;
        border: #e4e7ed solid 1px;
        border-left: 0;
        border-bottom: 0;
        transform: rotate(-45deg);
        z-index: 99999;
        background: #fff;
    }
</style>
