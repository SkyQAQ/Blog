<template>
  <div id="vedio_upload" class="vedio-upload">
    <el-dialog
      title="视频上传"
      :visible.sync="visible"
      :before-close="close"
      :close-on-click-modal="false"
      width="60%"
    >
      <el-form ref="form" :model="params" label-width="120px">
        <el-form-item label="视频描述">
          <el-input v-model="params.Description" maxlength="150" show-word-limit clearable></el-input>
        </el-form-item>
        <el-form-item label="上传视频">
          <el-upload
            class="upload-file"
            ref="upload_vedio"
            action="nonono"
            accept="video/*"
            :on-exceed="onExceed"
            :on-preview="handlePreview"
            :before-upload="beforeUpload"
            :on-success="onSuccess"
            :on-error="onError"
            :on-change="onChangeVedio"
            :on-remove="onRemoveVedio"
            :auto-upload="false"
            :limit="1"
            :disabled="loading"
          >
            <el-button slot="trigger" :disabled="disabled_vedio" size="small" type="primary">选取视频</el-button>尽量上传mp4格式
          </el-upload>
        </el-form-item>
        <br />
        <br />
        <el-form-item label="上传视频封面">
          <el-upload
            class="upload-file"
            ref="upload_poster"
            action="nonono"
            accept="image/*"
            :on-exceed="onExceed"
            :on-preview="handlePreview"
            :before-upload="beforeUpload"
            :on-success="onSuccess"
            :on-error="onError"
            :on-change="onChangePoster"
            :on-remove="onRemovePoster"
            :auto-upload="false"
            :limit="1"
            :disabled="loading"
          >
            <el-button slot="trigger" :disabled="disabled_poster" size="small" type="primary">选取封面</el-button>尽量上传jepg、png格式
          </el-upload>
        </el-form-item>
        <br />
        <br />
        <el-form-item label="是否公开">
          <el-switch v-model="params.IsPublic"></el-switch>
        </el-form-item>
      </el-form>
      <span slot="footer">
        <div class="upload-progress">
          <pre>{{uploadResult}}</pre>
        </div>
        <el-button :loading="loading" size="small" type="success" @click="submitUpload">上传到服务器</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
export default {
  name: "VedioUpload",
  data() {
    return {
      loading: false,
      fileList: [],
      uploadUrl: "",
      params: {},
      limit: {},
      uploadResult: "",
      description: "",
      disabled_vedio: false,
      disabled_poster: false
    };
  },
  props: {
    visible: {
      type: Boolean,
      required: true
    },
    urlPrefix: {
      type: String,
      required: false
    }
  },
  computed: {},
  mounted: function() {
    this.load();
  },
  methods: {
    load: function(moduleType, moduleId) {
      this.params = {
        Description: "",
        IsPublic: true
      };
      this.config = JSON.parse(
        localStorage.getItem(_const.Key_CommonConfig)
      ) || { Url: "", MaxLength: 0, MaxCount: 0 };
    },
    reset: function() {
      this.fileList = [];
      this.params = {};
      this.disabled_vedio = false;
      this.disabled_poster = false;
      this.$refs.upload_vedio.clearFiles();
      this.$refs.upload_poster.clearFiles();
      this.uploadResult = "";
    },
    submitUpload: function() {
      this.uploadResult = "";
      let apiUrl = this.urlPrefix + "api/vedioinfo/upload";
      if (!this.fileList) {
        wy.showErrorMssg("请选取视频！");
        return;
      }
      var data = new FormData();
      for (var i in this.fileList) {
        data.append("file", this.fileList[i].raw);
      }
      data.append("VedioInfoId", wy.NewGuid());
      data.append("Description", this.params.Description);
      data.append("IsPublic", this.params.IsPublic);
      this.loading = true;
      wy.post(apiUrl, data)
        .then(res => {
          wy.showSuccessMssg("上传成功！");
          this.reset();
          this.loading = false;
        })
        .catch(error => {
          wy.showErrorMssg(error);
          this.loading = false;
        });
    },
    handlePreview: function(file) {
      console.log(file);
    },
    beforeUpload: function(file) {},
    onChangeVedio: function(file, fileList) {
      if (file.size > this.config.MaxLength * 1024 * 1024) {
        this.uploadResult += file.name + ":文件大小超过限制 \n";
        for (var i in fileList) {
          if (fileList[i] == file) {
            fileList.splice(i, 1);
          }
        }
      }
      if (fileList && fileList.length > 0) {
        this.disabled_vedio = true;
        this.fileList.push(file);
      }
    },
    onRemoveVedio: function(file, fileList) {
      this.disabled_vedio = false;
      for (var i in this.fileList) {
        if (this.fileList[i] == file) {
          this.fileList.splice(i, 1);
        }
      }
    },
    onChangePoster: function(file, fileList) {
      if (file.size > this.config.MaxLength * 1024 * 1024) {
        this.uploadResult += file.name + ":文件大小超过限制 \n";
        for (var i in fileList) {
          if (fileList[i] == file) {
            fileList.splice(i, 1);
          }
        }
      }
      if (fileList && fileList.length > 0) {
        this.disabled_poster = true;
        this.fileList.push(file);
      }
    },
    onRemovePoster: function(file, fileList) {
      this.disabled_poster = false;
      for (var i in this.fileList) {
        if (this.fileList[i] == file) {
          this.fileList.splice(i, 1);
        }
      }
    },
    onSuccess: function(response, file, fileList) {},
    onError: function(error, file, fileList) {
      console.log(error);
      this.uploadResult = JSON.parse(error.message).mssg;
    },
    onExceed: function(files, fileList) {
      // 文件超数目限制钩子
    },
    close: function() {
      this.reset();
      this.$emit("close");
    }
  }
};
</script>

<style scoped>
.upload-file {
  height: 210px;
}

.upload-progress {
  text-align: left;
  margin-top: 5px;
}

.upload-progress pre {
  font-size: 12px;
  color: red;
}

.vedio-upload .upload-file {
  width: 80%;
}
</style>
