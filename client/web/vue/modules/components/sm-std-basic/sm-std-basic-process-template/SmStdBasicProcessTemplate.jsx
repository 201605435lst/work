import SmStdBasicProcessTemplateTree from './SmStdBasicProcessTemplateTree';
import SmStdBasicProjectItemRltProcessTemplate from './SmStdBasicProjectItemRltProcessTemplate';
import SmStdBasicProcessTemplateBasicInformation from './SmStdBasicProcessTemplateBasicInformation';
import './style';
export default {
  name: 'SmStdBasicProcessTemplate',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      transferDatas: null,
      dataSource: null,
      editData: null,
      tabListTitle: [
        {
          key: 'infor',
          tab: '基本信息',
        },
        {
          key: 'project',
          tab: '关联工程工项或单项工程',
        },
      ],
      titleKey: 'infor',
    };
  },
  created() {
    this.initAxios();
  },
  methods: {
    initAxios() {},
    onTabChange(key, type) {
      this[type] = key;
    },
    //给树传递的值
    transferData(data) {
      this.transferDatas = data;
    },
    //获得树传过来的数据
    async getRecord(record) {
      this.dataSource = record;
    },
    //获得基本信息表传递给树的数据
    getEditData(data) {
      this.editData = data;
    },
  },
  render() {
    return (
      <div class="sm-std-basic-process-template">
        <div class="std-left">
          <SmStdBasicProcessTemplateTree
            axios={this.axios}
            onRecord={this.getRecord}
            transferData={this.transferDatas}
            editData={this.editData}
            permissions={this.permissions}
          />
        </div>
        <div class="std-right">
          <a-card
            class="std-right-card"
            tabList={this.tabListTitle}
            activeTabKey={this.titleKey}
            onTabChange={key => this.onTabChange(key, 'titleKey')}
          >
            {this.titleKey === 'infor' ? (
              <p>
                {this.dataSource ? (
                  <SmStdBasicProcessTemplateBasicInformation
                    axios={this.axios}
                    datas={this.dataSource}
                    permissions={this.permissions}
                    onEditData={item => this.getEditData(item)}
                  />
                ) : (
                  <a-empty description={false} />
                )}
              </p>
            ) : (
              undefined
            )}
            {this.titleKey === 'project' ? (
              <p>
                {this.dataSource ? (
                  <SmStdBasicProjectItemRltProcessTemplate
                    axios={this.axios}
                    datas={this.dataSource}
                    permissions={this.permissions}
                    onDataValue={item => this.transferData(item)}
                  />
                ) : (
                  <a-empty description={false} />
                )}
              </p>
            ) : (
              undefined
            )}
          </a-card>
        </div>
      </div>
    );
  },
};
