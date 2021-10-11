import SmStdBasicIndividualProjectRltProjectItem from './component/SmStdBasicIndividualProjectRltProjectItem';
import SmStdBasicIndividualProjectBasicInformation from './component/SmStdBasicIndividualProjectBasicInformation';
import SmStdBasicIndividualProjectTree from './component/SmStdBasicIndividualProjectTree';
import './style';
export default {
  name: 'SmStdBasicIndividualProject',
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
          key: 'projectItem',
          tab: '关联工程工项',
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
      <div class="sm-std-basic-individual-project">
        <div class="std-left">
          <SmStdBasicIndividualProjectTree
            axios={this.axios}
            //获取树节点的基本信息
            onRecord={this.getRecord}
            editData={this.editData}
            transferData={this.transferDatas}
            permissions={this.permissions}
          />
        </div>
        <div class="std-right">
          <a-card
            class="std-right-card"
            tabList={this.tabListTitle}
            activeTabKey={this.titleKey}
            onTabChange={(key) => this.onTabChange(key, "titleKey")}
          >
            {this.titleKey == 'infor' ? (
              <p>
                {this.dataSource ? (
                  <SmStdBasicIndividualProjectBasicInformation
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
              <p>
                {this.dataSource ? (
                  <SmStdBasicIndividualProjectRltProjectItem
                    axios={this.axios}
                    datas={this.dataSource}
                    permissions={this.permissions}
                  />
                ) : (
                  <a-empty description={false} />
                )}
              </p>
            )}
          </a-card>
        </div>
      </div>
    );
  },
};
