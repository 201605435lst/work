import { pagination as paginationConfig } from '../../_utils/config';
import ApiMvdCategory from '../../sm-api/sm-std-basic/MVDCategory';
import { requestIsSuccess } from '../../_utils/utils';
import './style/index';

let apiMvdCategory = new ApiMvdCategory();
export default {
  name: 'SmStdBasicProductCategoryTree',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      dataSource: [], // 列表数据源
      record: null, //选中的记录

      pageIndex: 1,
      totalCount: 0,
      queryParams: {
        keyWords: null, //关键字
        isAll: true,
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  methods: {
    initAxios() {
      apiMvdCategory = new ApiMvdCategory(this.axios);
    },

    //刷新
    async refresh() {
      let response = await apiMvdCategory.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
        this.dataSource.forEach(item => (item.title = item.name + '（' + item.code + '）'));
        this.totalCount = response.data.totalCount;
      }
    },
  },
  render() {
    return (
      <a-card title="信息交换模板分类管理" style="overflow-y: scroll;max-height: 718px;width:300px;    height: 100%;">
        {/* <a-list dataSource={this.dataSource} style="overflow-y: scroll;max-height: 718px;width:300px"> 
         {this.dataSource.map(item => {
            return [
              <a-list-item style="color：#2f54eb">
                <a-list-item-meta>
                   <a
                    slot="title"
                    onClick={() => {
                      //提交选中的信息交换模板属性
                      this.$emit('record', item);
                    }}
                  >
                    {item.name + '[' + item.code + ']'}
                  </a>
                </a-list-item-meta>
              </a-list-item>,
            ];
          })} 
       </a-list>*/}
        <a-tree
          treeData={this.dataSource}
          replaceFields={{ title: 'title', key: 'id' }}
          onSelect={(selectedKeys, info) => {
            // let value = info.selectedNodes[0].data.props.dataRef;
            let value = info && info.selectedNodes && info.selectedNodes.length>0? info.selectedNodes[0].data.props.dataRef:'';
            this.$emit('record', value);
          }}
        />
        <a-input-search
          style="margin-top:10px"
          placeholder="请输入分类名称或代号"
          enter-button
          allowClear
          onSearch={item => {
            this.queryParams.keyWords = item;
            this.refresh();
          }}
        />
      </a-card>
    );
  },
};
