import moment from 'moment';
import { ModalStatus } from '../../_utils/enum';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, getSealType, getFileUrl, vP, vIf } from '../../_utils/utils';
import permissionsSmOa from '../../_permissions/sm-oa';
import ApiSeal from '../../sm-api/sm-oa/Seal';
import SmOaSealsModal from '../sm-oa-seals/SmOaSealsModal';

let apiSeal = new ApiSeal();

export default {
  name: 'SmOaSeals',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    multiple: { type: Boolean, default: true },//是否多选，
    selected: { type: Array, default: () => [] },//所选签章
    isSelect: {type: Boolean, default: false}, //是否为选择框状态
    personal:{ type: Boolean, default: false },//个人模式，只能选择属于自己的签章
  },

  data(){
    return{
      showTable: true, // 以表格的形式显示
      seals: [], // 列表数据源
      sealId:null,//签章id
      totalCount: 0,
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      queryParams:{
        keywords:null,
        personal:true,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
      selectedRowKeys: [], // 表格选中的行Ids
      iSelected: [],//已选签章
      sealName:null,//已选签章名字
    };
  },

  computed:{
    columns(){
      return[
        {
          title:'#',
          dataIndex:'index',
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'签章名称',
          dataIndex:'name',
        },
        {
          title:'签章类型',
          dataIndex:'signatureType',
          scopedSlots:{customRender:'signatureType'},
        },
        {
          title:'创建人',
          dataIndex:'creatorName',
        },
        {
          title:'创建时间',
          dataIndex:'creationTime',
          scopedSlots:{customRender:'creationTime'},
        },
        this.isSelect ? 
          {
            title:'是否可用',
            dataIndex:'enabled',
            scopedSlots: { customRender: 'enabled' },
          }
          :
          {
            title:'操作',
            dataIndex:'operations',
            width: 169,
            scopedSlots: { customRender: 'operations' },
            fixed: 'right',
          },
      ];
    },
    indeterminate() {
      return this.seals.filter(item => item.enabled == true).length > this.selectedRowKeys.length && this.selectedRowKeys.length > 0;
    },
    isAllCheck() {
      return this.seals.filter(item => item.enabled == true).length == this.selectedRowKeys.length && this.seals.filter(item => item.enabled == true).length > 0;
    },
  },

  watch: {
    selected: {
      handler: function (value, oldVal) {
        this.iSelected = value;
        this.selectedRowKeys = value.map(item => item.id);
      },
      immediate: true,
    },
    personal: {
      handler: function (value, oldVal) {
        this.queryParams.personal = value;
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  methods:{
    initAxios() {
      apiSeal = new ApiSeal(this.axios);
    },
    // 添加签章
    addSeal() {
      this.$refs.SmOaSealModal.add();
    },

    //删除
    remove(multiple, selectedRowKeys) {
      if (selectedRowKeys && selectedRowKeys.length > 0) {
        let _this = this;
        this.$confirm({
          title: tipsConfig.remove.title,
          content: h => (
            <div style="color:red;">
              {multiple ? '确定要删除这几条数据？' : tipsConfig.remove.content}
            </div>
          ),
          okType: 'danger',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiSeal.delete(selectedRowKeys);
              if (requestIsSuccess(response)) {
                _this.$message.success('文件已删除');
                _this.refresh();
                
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          },
          onCancel() { },
        });
      } else {
        this.$message.error('请选择要删除的签章！');
      }
    },

    async refresh(resetPage = true, page){
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiSeal.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.seals = response.data.items;
        this.totalCount = response.data.totalCount;
        if (page && this.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
          if (this.totalCount % this.queryParams.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.pageIndex = currentPage;
            this.refresh(false, this.pageIndex);
          }
        }
      }
      this.loading = false;
    },
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    //更新所选数据
    updateSelected(selectedRows) {
      if (!this.multiple) {
        this.iSelected = selectedRows;
      } else {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.seals.find(sealItem => sealItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedRowKeys) {
          let seals = this.seals.find(item => item.id === id);
          if (!!seals) {
            _selected.push(JSON.parse(JSON.stringify(seals)));
          }
        }

        this.iSelected = _selected;
      }
      this.$emit('change', this.iSelected);
    },

    async changeSelectedExamPaperIds(checkedValue){
      checkedValue.checked ? this.selectedRowKeys.push(checkedValue.value) : this.selectedRowKeys = this.selectedRowKeys.filter(item => item !== checkedValue.value);

      if(!this.multiple){
        if(checkedValue.value != null){
          let response = await apiSeal.get(checkedValue.value);
          if (requestIsSuccess(response)) {
            this.sealName = response.data.name;
          }
          this.updateSelected([{id:checkedValue.value,name:this.sealName}]);
        }else{
          this.$message.error('此签章不存在');
        }
      }
    },

    //解密
    decode(id,isPublic){
      let _this = this;
      this.$confirm({
        title: "确认解密",
        content: h => (
          <div style="color:red;">
            {'确定要解密这条数据？'}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSeal.update({id:id,isPublic:isPublic,type:1});
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },

    //加密
    encryption(id){
      this.$refs.SmOaSealModal1.update(id); 
    },

    //使签章有效
    effective(id,enabled){
      let _this = this;
      this.$confirm({
        title: "确认设为有效",
        content: h => (
          <div style="color:red;">
            {'确认将此签章设为有效？'}
          </div>
        ),
        okType: 'warning',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSeal.update({id:id,enabled:enabled,type:2});
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },

    //使签章无效
    noneffective(multiple,selectedRowKeys){
      if (selectedRowKeys && selectedRowKeys.length > 0) {
        let _this = this;
        this.$confirm({
          title: '确认设为无效',
          content: h => (
            <div style="color:red;">
              {multiple ? '确定要将这几条签章设为无效？' : '确认将此签章设为无效？'}
            </div>
          ),
          okType: 'danger',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiSeal.update({ids:selectedRowKeys,type:3});
              if (requestIsSuccess(response)) {
                _this.$message.success('操作成功');
                _this.selectedRowKeys = [];
                _this.refresh();
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          },
          onCancel() { },
        });
      } else {
        this.$message.error('请选择要设为无效的签章！');
      }
    },

  },

  render() {
    let tableContent = (
      <div>
        <a-table
          columns={this.columns}
          dataSource={this.seals}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          pagination={false}
          rowSelection={{
            columnWidth: 30,
            type: this.multiple ? 'checkbox' : 'radio',
            selectedRowKeys: this.selectedRowKeys,
            onChange: (key,items) => {
              this.selectedRowKeys = key;
              this.updateSelected(items);
            },
            getCheckboxProps: a => {
              return {
                props: {
                  disabled:!a.enabled,
                  defaultChecked: this.selectedRowKeys.includes(a),
                },
              };
            },
          }}
          customRow={(record) => {
            return {
              style: {
                color: record.enabled == false ? "#C0C0C0" : undefined,
              },
            };
          }}
          scroll={!this.multiple ? { y: 400 } : undefined}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              
              signatureType: (text, record) => {
                let type = record.type != null ? getSealType(record.type) : '未知类型';
                return (<a-tooltip placement='topLeft' title={type}><span>{type}</span></a-tooltip>);
              },

              creationTime:(text,record) => {
                let creationTime = record.creationTime ? moment(record.creationTime).format('YYYY-MM-DD HH:mm:ss') : '';
                return (<a-tooltip placement='topLeft' title={creationTime}><span>{creationTime}</span></a-tooltip>);
              },
              enabled:(text,record) => {
                return record.enabled ? "是" : "否";
              },

              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a-tooltip placement='topLeft' arrowPointAtCenter title={record.isPublic == false ? "有密" : "无密"}>
                        <a onClick={() => record.isPublic == false ? this.decode(record.id,record.isPublic) : this.encryption(record.id,record.isPublic)}>
                          {record.isPublic == false ? 
                            <si-password style={"font-size: 21px;color: red"}/> 
                            : <si-unlock style={"font-size: 21px;"}/>
                          }
                        </a>
                      </a-tooltip>,
                      vP(this.permissions, permissionsSmOa.Seals.Lock),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmOa.Seals.Lock) &&
                      vP(this.permissions, [permissionsSmOa.Seals.Delete,permissionsSmOa.Seals.Efficiency,permissionsSmOa.Seals.RestPSW]),
                    )}
                    
                    {vIf(
                      <a-tooltip placement='topLeft' arrowPointAtCenter title={"删除"}>
                        <a
                          onClick={() => {this.remove(false, [record.id]);}}
                        >
                          <si-ashbin style={"font-size: 21px;color: red"}/>
                        </a>
                      </a-tooltip>,
                      vP(this.permissions, permissionsSmOa.Seals.Delete),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmOa.Seals.Delete) &&
                      vP(this.permissions, [permissionsSmOa.Seals.Efficiency,permissionsSmOa.Seals.RestPSW]),
                    )}
                    {vIf(
                      <a-tooltip placement='topLeft' arrowPointAtCenter title={record.enabled == true ? "有效" : "无效"}>
                        <a onClick={() => record.enabled == false ? this.effective(record.id,record.enabled) : this.noneffective(false,[record.id])}>
                          {record.enabled == true ? 
                            <si-success style={"font-size: 21px;"}/>
                            : <si-reduce style={"font-size: 21px;color: red"}/>
                          }
                        </a>
                      </a-tooltip>,
                      vP(this.permissions, permissionsSmOa.Seals.Efficiency),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmOa.Seals.Efficiency) &&
                      vP(this.permissions, permissionsSmOa.Seals.RestPSW),
                    )}
                    {vIf(
                      <a-tooltip placement='topLeft' arrowPointAtCenter title={record.isPublic == true ? undefined : "重置密码"}>
                        <a onClick={() => record.isPublic == false ? this.encryption(record.id,!record.isPublic) : undefined}>
                          {record.isPublic == true ? 
                            <si-set style={"font-size: 21px;color: #A9A9A9"}/>
                            : <si-set style={"font-size: 21px;"}/>
                          }
                        </a>
                      </a-tooltip>,
                      vP(this.permissions, permissionsSmOa.Seals.RestPSW),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        
      </div>
    );

    let thumContent = (
      <div class={'f-thum-content'}>
        {this.seals.map(item => {
          return (
            <div
              class={'f-thum-item'}
            >
              <div class="f-thum">

                <div class="f-thum-file">
                  <img width="100%" height="100%" src={getFileUrl(item.image.url)} alt="签章" />
                </div>
                {this.multiple == true ?
                  <a-checkbox-group value={this.selectedRowKeys}>
                    <a-checkbox disabled={!item.enabled} value={item.id} onChange={checkedValue => this.changeSelectedExamPaperIds(checkedValue.target)}>
                      {item.name}
                    </a-checkbox>
                  </a-checkbox-group>
                  :
                  <a-radio-group value={this.selectedRowKeys.length > 0 ? this.selectedRowKeys[0] : ''} onChange={checkedValue => this.changeSelectedExamPaperIds(checkedValue.target)}>
                    <a-radio disabled={!item.enabled} value={item.id}>
                      {item.name}
                    </a-radio>
                  </a-radio-group>}
                <p style={!item.enabled ? 'color:#D3D3D3' : undefined}>
                  类型：
                  <a-tooltip placement="topRight">
                    <template slot="title">
                      <span>
                        {getSealType(item.type)}
                      </span>
                    </template>
                    {getSealType(item.type)}
                  </a-tooltip>
                </p>
                <p style={!item.enabled ? 'color:#D3D3D3' : undefined}>
                授权用户：
                  <a-tooltip placement="bottom">
                    <template slot="title">
                      <span>
                        {item.authorizedName}
                      </span>
                    </template>
                    {item.authorizedName}
                  </a-tooltip>
                </p>

              </div>
            </div>
          );
        })}
      </div>
    );

    return (
      <div class="sm-oa-seal">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams = {    
              keywords:null,
            };
            this.refresh();
          }}
        >
          <a-form-item label="关键字" style={'margin-left: 14px'}>
            <a-input
              placeholder="请输入签章名称"
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          {this.multiple ?
            <template slot="buttons">
              {this.showTable == false ?
                <a-button type="primary">
                  <a-checkbox 
                    checked={this.isAllCheck}
                    indeterminate={this.indeterminate}
                    onChange={value => {
                      this.selectedRowKeys = value.target.checked ? this.seals.filter(item => item.enabled == true).map(item => item.id) : [];
                    }}
                  >
                    <span style={"color:white"}>全选</span>
                  </a-checkbox>
                </a-button>
                : undefined}
              {vIf(
                <a-button type="primary"  icon="plus" onClick={this.addSeal}>
                    新建
                </a-button>,
                vP(this.permissions, permissionsSmOa.Seals.Create),
              )}
              {vIf(
                <a-button type="danger"  icon="delete" onClick={() => this.remove(true, this.selectedRowKeys)}>
                批量删除
                </a-button>,
                vP(this.permissions, permissionsSmOa.Seals.Delete),
              )}
              {vIf(
                <a-button type="primary" onClick={() => this.noneffective(true, this.selectedRowKeys)}>
                设为无效
                </a-button>,
                vP(this.permissions, permissionsSmOa.Seals.Efficiency),
              )}
            </template>
            : undefined}
          <template slot="buttons">
            <a-tooltip placement='topLeft' title={this.showTable ? '表格模式' : '矩阵模式'}>
              <a-icon
                style={"float: right;font-size: 30px;"}
                onClick={() => {
                  this.showTable = !this.showTable;
                }}
                type={this.showTable ? 'border-outer' : 'unordered-list'}
              />
            </a-tooltip>
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        {this.showTable ? tableContent : thumContent}
        
        {/* 分页器 */}
        <a-pagination
          style="float:right; margin-top:10px"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />

        <SmOaSealsModal
          axios={this.axios}
          ref='SmOaSealModal'
          onSuccess={() => this.refresh()}
        ></SmOaSealsModal>
        <SmOaSealsModal
          ref='SmOaSealModal1'
          axios={this.axios}
          sealId={this.sealId}
          onSuccess={() => {this.refresh();this.sealId = null;}}
        ></SmOaSealsModal>
      </div>
      
    );
  },
};