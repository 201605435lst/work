import SmStdBasicComponentCategoryTree from './component/SmStdBasicComponentCategoryTree';
import SmStdBasicComponentCategoryClassification from './component/SmStdBasicComponentCategoryClassification';
import SmStdBasicComponentCategoryBasicInformation from './component/SmStdBasicComponentCategoryBasicInformation';
import SmStdBasicComponentCategoryRltQuota from './component/SmStdBasicComponentCategoryRltQuota';
import SmStdBasicComponentCategoryRltMaterials from './component/SmStdBasicComponentCategoryRltMaterials';
import SmStdBasicComponentCategoryRltProperty from './component/SmStdBasicComponentCategoryRltProperty';

import './style';
export default {
  name: 'SmStdBasicComponentCategory',
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
          key: 'class',
          tab: '下级分类',
        },
        {
          key: 'relevanceQuota',
          tab: '关联定额',
        },
        {
          key: 'relevanceMaterials',
          tab: '关联材料',
        },
        {
          key: 'relevanceProperty',
          tab: '关联属性',
        },
      ],
      titleKey: 'infor',
    };
  },
  // watch: {
  //   dataSource: {
  //     handler: function (val, oldVal) {
  //     },

  //   },
  // },
  created() {
    this.initAxios();
    // this.refresh();
  },
  methods: {

    initAxios() {

    },
    onTabChange(key, type) {
      this[type] = key;
    },
    //给树传递的值
    transferData(data) {
      this.transferDatas = data;
    },
    //获得树传过来的数据
    async getRecord(record) {
      console.log(record);
      this.dataSource = record;
    },
    //获得基本信息表传递给树的数据
    getEditData(data) {
      this.editData = data;
    },
  },
  render() {
    // 定义基本框架
    return (
      <div class="sm-std-basic-component-category">
        {/* 左侧 */}
        <div class="std-left">
          <SmStdBasicComponentCategoryTree
            axios={this.axios}
            onRecord={this.getRecord}
            transferData={this.transferDatas}
            editData={this.editData}
            permissions={this.permissions}
          />
        </div>
        {/* 右侧 */}
        <div class="std-right">
          <a-card
            class="std-right-card"
            tabList={this.tabListTitle}
            activeTabKey={this.titleKey}
            onTabChange={(key) => this.onTabChange(key, "titleKey")}
          >
            {this.titleKey === 'infor' ?
              <p>
                {this.dataSource ?
                  <SmStdBasicComponentCategoryBasicInformation
                    axios={this.axios} datas={this.dataSource}
                    permissions={this.permissions}
                    onEditData={(item) => this.getEditData(item)} />
                  : <a-empty description={false} />}

              </p> : undefined}
            {this.titleKey === 'class' ?
              <p>
                {this.dataSource ?
                  <SmStdBasicComponentCategoryClassification
                    axios={this.axios} datas={this.dataSource}
                    permissions={this.permissions}
                    onDataValue={(item) => this.transferData(item)}
                  />
                  : <a-table data-source={[]} />}
              </p> : undefined}
            {this.titleKey === 'relevanceQuota' ?
              <p>
                {this.dataSource ?
                  <SmStdBasicComponentCategoryRltQuota
                    axios={this.axios} datas={this.dataSource}
                    permissions={this.permissions}
                    onDataValue={(item) => this.transferData(item)}
                  />
                  : <a-empty description={false} />}
              </p> : undefined}

            {this.titleKey === 'relevanceMaterials' ?
              <p>
                {this.dataSource ?
                  <SmStdBasicComponentCategoryRltMaterials
                    axios={this.axios} datas={this.dataSource}
                    permissions={this.permissions}
                    onDataValue={(item) => this.transferData(item)}
                  />
                  : <a-empty description={false} />}
              </p> : undefined}

            {this.titleKey === 'relevanceProperty' ?
              <p>
                {this.dataSource ?
                  <SmStdBasicComponentCategoryRltProperty
                    axios={this.axios} datas={this.dataSource}
                    permissions={this.permissions}
                    onDataValue={(item) => this.transferData(item)}
                    componentCategoryId={this.dataSource.id}
                  />
                  : <a-empty description={false} />}
              </p> : undefined
            }

          </a-card>
        </div>
      </div>
    );
  },
};

