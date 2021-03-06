/*
 * author kcz
 * date 2019-12-31
 * description 上传文件组件
 */
export default {
  name: 'KUploadFile',
  // eslint-disable-next-line vue/require-prop-types
  props: ['record', 'value', 'parentDisabled'],
  data() {
    return {
      fileList: [],
    };
  },

  computed: {
    optionsData() {
      try {
        return JSON.parse(this.record.options.data);
      } catch (e) {
        return {};
      }
    },
  },
  watch: {
    value: {
      // value 需要深度监听及默认先执行handler函数
      handler(val) {
        if (val) {
          this.setFileList();
        }
      },
      immediate: true,
      deep: true,
    },
  },
  methods: {
    setFileList() {
      // 当传入value改变时，fileList也要改变
      // 如果传入的值为字符串，则转成json
      if (typeof this.value === 'string') {
        this.fileList = JSON.parse(this.value);
        // 将转好的json覆盖组件默认值的字符串
        this.handleSelectChange();
      } else {
        this.fileList = this.value;
      }
    },
    handleSelectChange() {
      setTimeout(() => {
        const arr = this.fileList.map(item => {
          if (typeof item.response !== 'undefined') {
            const res = item.response;
            return {
              type: 'file',
              name: item.name,
              status: item.status,
              uid: res.data.fileId || Date.now(),
              url: res.data.url || '',
            };
          } else {
            return {
              type: 'file',
              name: item.name,
              status: item.status,
              uid: item.uid,
              url: item.url || '',
            };
          }
        });

        this.$emit('change', arr);
        this.$emit('input', arr);
      }, 10);
    },
    handlePreview(file) {
      // 下载文件
      this.getBlob(file.url || file.thumbUrl).then(blob => {
        this.saveAs(blob, file.name);
      });
    },
    /**
     * 获取 blob
     * url 目标文件地址
     */
    getBlob(url) {
      return new Promise(resolve => {
        const xhr = new XMLHttpRequest();

        xhr.open('GET', url, true);
        xhr.responseType = 'blob';
        xhr.onload = () => {
          if (xhr.status === 200) {
            resolve(xhr.response);
          }
        };

        xhr.send();
      });
    },
    /**
     * 保存 blob
     * filename 想要保存的文件名称
     */
    saveAs(blob, filename) {
      if (window.navigator.msSaveOrOpenBlob) {
        navigator.msSaveBlob(blob, filename);
      } else {
        const link = document.createElement('a');
        const body = document.querySelector('body');
        link.href = window.URL.createObjectURL(blob);
        link.download = filename;

        // fix Firefox
        link.style.display = 'none';
        body.appendChild(link);

        link.click();
        body.removeChild(link);

        window.URL.revokeObjectURL(link.href);
      }
    },
    remove() {
      this.handleSelectChange();
    },
    beforeUpload(e, files) {
      if (files.length + this.fileList.length > this.record.options.limit) {
        this.$message.warning(`最大上传数量为${this.record.options.limit}`);
        files.splice(this.record.options.limit - this.fileList.length);
      }
    },
    handleChange(info) {
      this.fileList = info.fileList;
      if (info.file.status === 'done') {
        const res = info.file.response;
        if (res.code === 0) {
          this.handleSelectChange();
        } else {
          this.fileList.pop();
          this.$message.error(`文件上传失败`);
        }
      } else if (info.file.status === 'error') {
        this.$message.error(`文件上传失败`);
      }
    },
  },
  /*
   * @Description: 对上传文件组件进行封装
   * @Author: kcz
   * @Date: 2020-03-17 12:53:50
   * @LastEditors: kcz
   * @LastEditTime: 2020-03-29 22:03:27
   */
  render() {
    return (
      <div style={{ width: this.record.options.width }}>
        {!this.record.options.drag ? (
          <a-upload
            disabled={this.record.options.disabled || this.parentDisabled}
            name={this.record.model}
            multiple={this.record.options.multiple}
            data={this.optionsData}
            file-list={this.fileList}
            action={this.record.options.action}
            remove={this.remove}
            before-upload={this.beforeUpload}
            onPreview={this.handlePreview}
            onChange={this.handleChange}
          >
            {this.fileList.length < this.record.options.limit ? (
              <a-button disabled={this.record.options.disabled || this.parentDisabled}>
                <a-icon type="upload" /> {this.record.options.placeholder}
              </a-button>
            ) : (
              undefined
            )}
          </a-upload>
        ) : (
          <a-upload-dragger
            disabled={this.record.options.disabled || this.parentDisabled}
            name={this.record.model}
            multiple={this.record.options.multiple}
            file-list={this.fileList}
            data={this.optionsData}
            action={this.record.options.action}
            remove={this.remove}
            before-upload={this.beforeUpload}
            onPreview={this.handlePreview}
            onChange={this.handleChange}
          >
            <p class="ant-upload-drag-icon">
              <a-icon type="cloud-upload" />
            </p>
            <p class="ant-upload-text">单击或拖动文件到此区域</p>
          </a-upload-dragger>
        )}
      </div>
    );
  },
};
