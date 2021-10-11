import { requestIsSuccess } from '../../_utils/utils';
import {  treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiProjectItem from '../../sm-api/sm-std-basic/ProjectItem';
import { dropdownStyle } from '../../_utils/config';

let apiProjectItem = new ApiProjectItem();

export default {
  name: 'SmStdBasicProjectItemListSelect',
  model: {
    prop: 'value',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String] }, //返回值
    disabled: { type: Boolean, default: false }, //是否禁用
    treeCheckable: { type: Boolean, default: true }, //设置多选还是单选
    treeCheckStrictly: { type: Boolean, default: false }, //父子级是否严格
    parentDisabled: { type: Boolean, default: false }, //父级是否禁用
    placeholder: { type: String, default: '请选择' },
    maxTagCount: { type: Number, default: 3 }, //多选状态下最多显示tag数
    allowClear: { type: Boolean, default: true }, //是否清除
    showSearch: { type: Boolean, default: false }, //是否显示搜索
  },

  data() {
    return {
      individualProjects: [], // 数据源
      iValue: [],
      individualProjectsFlat: [], //选中的单项工程
      isSearch: false, //树选择框是否处于搜索状态
    };
  },

  computed: {},

  watch: {
    value: {
      handler: async function(val, oldVal) {
        this.initAxios();
        if (this.isSearch) {
          await this.refresh(null, true);
          this.isSearch = false;
        } else {
          await this.refresh();
        }
        this.setValue();
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    // this.refresh(null, true);
  },

  methods: {
    initAxios() {
      apiProjectItem = new ApiProjectItem(this.axios);
    },
    isValueLoading() {
      let refresh = false;
      // 当是多选的时候
      if (this.value instanceof Array) {
        if (this.value.length > 0) {
          // 保证数组里面的所有数据已经加载
          if (this.value.some(id => this.individualProjectsFlat.find(x => x.id == id) == null)) {
            refresh = true;
          }
        } else {
          if (this.value.length == 0 && this.individualProjectsFlat.length == 0) {
            refresh = true;
          }
        }
      }
      // 当是单选的时候
      else {
        if (this.value) {
          if (this.individualProjectsFlat.find(x => x.id === this.value) == null) {
            refresh = true;
          }
        } else {
          if (!this.value && this.individualProjectsFlat.length == 0) {
            refresh = true;
          }
        }
      }
      return refresh;
    },
    // // 当选择框已经有值的时候，判断需不需要重新加载数据
    // isValueLoading() {
    //   let refresh = false;
    //   // 当是多选的时候
    //   if (this.value instanceof Array) {
    //   }
    //   // 当是单选的时候
    //   else {
    //   }
    //   return refresh;
    // },
    //初始化页面加载数据
    async refresh(keyWords, isReset) {
      let isValueLoading = isReset ? true : await this.isValueLoading();
      if (isValueLoading) {
        let response = await apiProjectItem.getList({
          ids: !keyWords
            ? this.value instanceof Array
              ? this.value
              : this.value
                ? [this.value]
                : []
            : [],
          isAll: true,
          keyWords: keyWords ? keyWords : '',
        });
    
        if (requestIsSuccess(response) && response.data.items) {
          this.individualProjects = [];
          this.individualProjectsFlat = [];
          response.data.items.map(item=>
          {
            item.children=null;
          });
          let _quotaCategorys = treeArrayItemAddProps(response.data.items, 'children', [
            {
              targetProp: 'title',
              handler: item => {
                let result = item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name;
                return result;
              },
            },
            // { sourceProp: 'name', targetProp: 'title' },
            { sourceProp: 'id', targetProp: 'value' },
            { sourceProp: 'id', targetProp: 'key' },
            {
              targetProp: 'isLeaf',
              handler: item => {
                return item.children === null ? true : false;
              },
            },
          ]);
          this.individualProjectsFlat = treeArrayToFlatArray(_quotaCategorys);
    
          this.individualProjects = _quotaCategorys;
        }
      }
    },
    // //初始化页面加载数据
    // async refresh(keyWords, isReset) {
    //   let isValueLoading = isReset ? true : await this.isValueLoading();
    //   //是否刷新
    //   if (isValueLoading) {
    //     let response = await apiProjectItem.getList({
    //       isAll: true,
    //       keyWord: keyWords ? keyWords : '',
    //     });
    //     if (requestIsSuccess(response)) {
    //       this.individualProjects = [];
    //       let _individualProjects = treeArrayItemAddProps(response.data.items, 'children', [
    //         // {
    //         //   targetProp: 'title',
    //         //   handler: item => {
    //         //     let result = item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name;
    //         //     return result;
    //         //   },
    //         // },
    //         { sourceProp: 'name', targetProp: 'title' },
    //         { sourceProp: 'id', targetProp: 'value' },
    //         { sourceProp: 'id', targetProp: 'key' },
    //       ]);
    //       this.individualProjects = _individualProjects;
    //     }
    //   }
    // },

    //搜索功能
    async onSearch(value) {
     
      await this.refresh(value, true);
      this.isSearch = true;
      if (!this.treeCheckable) {
        this.setValue();
      }
    },
    //异步加载数据
    async onLoadData(treeNode) {
      if (treeNode.dataRef.children && treeNode.dataRef.children.length == 0) {
        let response = await apiProjectItem.getList({
          ids: [],
          isAll: true,
        });
        if (requestIsSuccess(response) && response.data.items) {
          response.data.items.map(item=>{item.children=null;});
          this.individualProjectsFlat = this.individualProjectsFlat.concat(response.data.items);
  
          treeNode.dataRef.children = treeArrayItemAddProps(response.data.items, 'children', [
            {
              targetProp: 'title',
              handler: item => {
                let result = item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name;
                return result;
              },
            },
            { sourceProp: 'id', targetProp: 'value' },
            { sourceProp: 'id', targetProp: 'key' },
            {
              targetProp: 'isLeaf',
              handler: item => {
                return item.children === null ? true : false;
              },
            },
          ]);
        }
      }
    },
    // 多选模式下，value 值格式为：{value,label}格式
    async setValue() {
      let result = await this.processData(this.individualProjectsFlat, this.value);
      if (result) {
        if (this.treeCheckable) {
          this.iValue = this.value
            ? this.individualProjectsFlat
              .filter(item => {
                if (this.value.indexOf(item.id) > -1) {
                  return true;
                }
              })
              .map(item => {
                return {
                  value: item.id,
                  label: item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name,
                };
              })
            : [];
        } else {
          this.iValue = this.value;
        }
      } else {
        this.iValue = null;
      }
    },
    // 判断传过来的id是否在数据中
    processData(array, value) {
      let data = false;
      try {
        array.forEach((item, index, arr) => {
          //当是多选的情况，value是数组，单选是字符串
          if (this.treeCheckable && value && value.some(values => item.id == values)) {
            data = true;
            throw new Error('error');
          } else {
            if (item.id == value) {
              data = true;
              throw new Error('error');
            }
          }
          if (item.children != null && item.children.length > 0) {
            this.processData(item.children, value);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
      return data;
    },
  },
  // 多选模式下，value 值格式为：{value,label}格式
  // async setValue() {
  //   let result = await this.processData(this.individualProjectsFlat, this.value);
  //   if (result) {
  //     if (this.treeCheckable) {
  //       this.iValue = this.value
  //         ? this.individualProjectsFlat
  //             .filter(item => {
  //               if (this.value.indexOf(item.id) > -1) {
  //                 return true;
  //               }
  //             })
  //             .map(item => {
  //               return {
  //                 value: item.id,
  //                 label: item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name,
  //               };
  //             })
  //         : [];
  //     } else {
  //       this.iValue = this.value;
  //     }
  //   } else {
  //     this.iValue = null;
  //   }
  // },
  // // 判断传过来的id是否在数据中
  // processData(array, value) {
  //   let data = false;
  //   try {
  //     array.forEach((item, index, arr) => {
  //       //当是多选的情况，value是数组，单选是字符串
  //       if (this.treeCheckable && value.some(values => item.id == values)) {
  //         data = true;
  //         throw new Error('error');
  //       } else {
  //         if (item.id == value) {
  //           data = true;
  //           throw new Error('error');
  //         }
  //       }
  //       if (item.children != null && item.children.length > 0) {
  //         this.processData(item.children, value);
  //       }
  //     });
  //   } catch (e) {
  //     if (e.message != 'error') throw e;
  //   }
  //   return data;
  // },
  // },
  render() {
    return (
      <a-tree-select
        treeCheckable={this.treeCheckable}
        treeCheckStrictly={this.treeCheckStrictly}
        treeNodeFilterProp="title"
        maxTagCount={this.maxTagCount}
        dropdownStyle={dropdownStyle}
        disabled={this.disabled}
        allowClear={this.allowClear}
        value={this.iValue}
        showCheckedStrategy="SHOW_ALL"
        maxTagCount={this.maxTagCount}
        treeData={this.individualProjects}
        showSearch={this.showSearch}
        loadData={this.onLoadData}
        onChange={value => {
          this.iValue = value;
          let ids = this.treeCheckable ? value.map(item => item.value) : value;
          this.$emit('input', ids);
          this.$emit('change', ids);
        }}
        placeholder={this.disabled ? '' : this.placeholder}
        style="width: 100%"
        onSearch={value => {
          this.onSearch(value);
        }}
      ></a-tree-select>
    );
  },
};
