import { ModalStatus } from '../../../_utils/enum';
import SmOaContractSelectAssociatedDocument from './SmOaContractSelectAssociatedDocument';
import '../style';


export default {
  name: 'SmOaContractSelectAssociatedDocumentModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
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
  },
  render() {
    return (
      <div class="sm-oa-contract-select-associated-document-model">
        <a-modal
          class="sm-oa-contract-select-associated-document-m"
          visible={this.visible}
          destroyOnClose={true}
          okText="保存"
          maskClosable={false}
          width={800}
          closable={false}
          cancelText="关闭"
          onCancel={() => this.cancel()}
          onOk={() => console.log("确定")}
          {...{
            scopedSlots: {
              title: (record) => {
                return (
                  <div class="title-search">
                    <div class="title">选择关联文档</div>
                    <div class="search-button">
                      <div class="search">
                        <a-input
                          class="input"
                          placeholder={'请输入主题'}
                        />
                      </div>
                      <div>
                        <a-button type="primary" class="button">查询</a-button>
                      </div>
                    </div>
                  </div>
                );
              },

            },
          }}
        >
          {/* <a slot="closeIcon" href="#"><a-input></a-input></a> */}
          <SmOaContractSelectAssociatedDocument
            axios={this.axios}
          />
        </a-modal>
      </div>
    );
  },
};