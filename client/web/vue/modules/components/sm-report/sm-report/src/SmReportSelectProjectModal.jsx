import { ModalStatus } from '../../../_utils/enum';
import SmReportSelectProject from './SmReportSelectProject';
import '../style';
export default {
  name: 'SmReportSelectProjectModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      keyWords:null,//关键字查询
      projectKeyWord:null,//项目名字
    };
  },
  computed: {
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },

  },
  watch: {},
  async created() {
  },
  methods: {
    select() {
      this.status = ModalStatus.Add;
    },
    cancel() {
      this.status = ModalStatus.Hide;
    },
    refresh(){
      this.keyWords=this.projectKeyWord;
    },
  },
  render() {
    return (
      <div class="sm-report-select-model">
        <a-modal
          class="sm-report-select-model-m"
          visible={this.visible}
          destroyOnClose={true}
          okText="取消"
          maskClosable={false}
          width={800}
          closable={false}
          cancelText="关闭"
          onCancel={() => this.cancel()}
          onOk={() => this.cancel()}
          {...{
            scopedSlots: {
              title: (record) => {
                return (
                  <div class="title-search">
                    <div class="title">项目选择</div>
                    <div class="search-button">
                      <div class="search">
                        <a-input
                          class="input"
                          onPressEnter={this.refresh}
                          placeholder={'请输入项目名称/项目编号'}
                          onChange={(event) => {
                            this.projectKeyWord = event.target.value;
                            this.refresh();
                          }}
                        />
                      </div>
                      <div>
                        <a-button type="primary" class="button" onClick={(event)=>this.refresh()}>查询</a-button>
                      </div>
                    </div>
                  </div>
                );
              },

            },
          }}
        >
          {/* <a slot="closeIcon" href="#"><a-input></a-input></a> */}
          <SmReportSelectProject
            axios={this.axios}
            keyWords={this.keyWords}
            onRecordProject={(item) => {
              this.$emit("recordProject", item);
              this.cancel();
            }}
          />
        </a-modal>
      </div>
    );
  },
};