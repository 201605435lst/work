/**
 * 说明：技术交底
 * 作者：easten
 */
import { requestIsSuccess, getSupplierType, vIf, vP, getFileUrl } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiDisclose from '../../sm-api/sm-technology/Disclose';
import ApiQRcode from '../../sm-api/sm-common/QRcode';
import moment from 'moment';
import FileSaver from 'file-saver';
import FileSelect from './src/FileSelect';
import EditModal from './src/EditModal';
import { FileSizeTrans } from '../../sm-file/sm-file-manage/src/common';
import SmVideo from '../../sm-common/sm-video';
import { Modal } from 'ant-design-vue';
import QrCode from '../../sm-common/sm-qrcode';
import permissionsTechnology from '../../_permissions/sm-technology';
import './style';
let apiDisclose = new ApiDisclose();
let apiQRcode = new ApiQRcode();
export default {
  name: 'SmTechnologyDisclose',
  components: {},
  props: {
    axios: { type: Function, default: null },
    type: { type: String, default: '' },  // security  安全技术交底内容
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        keyWord: '',
        type: '',
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dataSource: null,
      record: null,
      selectedRowKeys: [],
      selectedRows: [],
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'xh',
          width: 60,
          ellipsis: true,
          // align: 'center',
          scopedSlots: { customRender: 'xh' },
        },
        {
          title: '视频名称',
          dataIndex: 'name',
          ellipsis: true,
          width: 400,
          // align: 'center',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '视频大小',
          dataIndex: 'size',
          ellipsis: true,
          // align: 'center',
          scopedSlots: { customRender: 'size' },
        },
        {
          title: '创建时间',
          dataIndex: 'creationTime',
          scopedSlots: { customRender: 'creationTime' },
          ellipsis: true,
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: '140',
          // align: 'center',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {},
  created() {
    apiDisclose = new ApiDisclose(this.axios);
    apiQRcode = new ApiQRcode(this.axios);
    this.refresh();
  },
  methods: {
    // 刷新
    async refresh(resetPage = true, page) {
      this.queryParams.type = this.type === 'security' ? 1 : 0;
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      // 获取数据
      let response = await apiDisclose.getList({
        ...this.queryParams,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
      });
      if (requestIsSuccess(response)) {
        this.loading = false;
        this.dataSource = response.data.items;
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
    },
    // 新增
    add() {
      this.$refs.fileSelect.add();
    },
    // 删除
    delete(record) {
      let _this = this;
      if (_this.selectedRowKeys == null || _this.selectedRowKeys.length == 0) {
        this.$message.warning('请选择需要删除的数据');
        return;
      }

      _this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content} </div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiDisclose.deleteRange(_this.selectedRowKeys);
            if (requestIsSuccess(response)) {
              _this.refresh(true);
              _this.selectedRowKeys = [];
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    // 导出
    async export() {
      if (this.selectedRowKeys.length === 0) {
        this.$message.error('请选择需要导出的数据');
        return false;
      }
      this.selectedRows.forEach(async item => {
        let url = getFileUrl(item.url);
        let content = {
          key: "video",
          value: url,
        };
        let response = await apiQRcode.download(JSON.stringify(content));
        if (requestIsSuccess(response)) {
          if (response.data.byteLength != 0) {
            FileSaver.saveAs(
              new Blob([response.data], { type: 'application/image' }),
              `${item.name}.png`,
            );
          }
        }
      });
    },
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    // 记录编辑
    edit(record) {
      this.$refs.editModal.edit(record);
    },
    // 查看
    view(record, qrcode) {
      let url = getFileUrl(record.url);
      let content = {
        key: "video",
        value: url,
      };

      if (qrcode) {
        Modal.success({
          title: `${record.name}-交底二维码`,
          class: "sm-qrcode-container",
          okText: '确定',
          content: <QrCode style='text-align:center' axios={this.axios} show content={JSON.stringify(content)} />,
        });
      } else {
        this.$refs.player.preview(true, url, record.name);
      }
    },

    //更新所选数据
    updateSelected() {
      // 过滤出其他页面已经选中的
      let _selected = [];
      for (let item of this.selectedRows) {
        let target = this.dataSource.find(subItem => subItem.id === item.id);
        if (!target) {
          _selected.push(item);
        }
      }

      // 把当前页面选中的加入
      for (let id of this.selectedRowKeys) {
        let data = this.dataSource.find(item => item.id === id);
        if (!!data) { // !! 是 js 里面  (a!=null&&typeof(a)!=undefined&&a!='') 的简略写法
          _selected.push(JSON.parse(JSON.stringify(data))); // 为了保证 _selected 改变(不这样写拿到的是内存的引用……)
        }
      }

      this.selectedRows = _selected;
    },
  },
  render() {
    return (
      <div class="sm-material-contract">
        <div class="head">
          <a-row>
            <a-col span={8} style="display:flex;width:100%;justify-content:space-between;">
              <div class="m-button-container">
                {vIf(
                  <a-button
                    style="width:80px;margin-left:5px"
                    type="primary"
                    size="default"
                    onClick={() => this.add()}
                  >
                    上传
                  </a-button>,
                  vP(this.permissions, permissionsTechnology.Discloses.Upload),
                )}
                {vIf(
                  <a-button
                    style="width:80px;margin-left:5px"
                    type="danger"
                    size="default"
                    onClick={() => this.delete()}
                  >
                    删除
                  </a-button>,
                  vP(this.permissions, permissionsTechnology.Discloses.Delete),
                )}
                {vIf(
                  <a-button
                    style="width:100px;margin-left:5px"
                    color="red"
                    size="default"
                    onClick={() => this.export()}
                  >
                    导出二维码
                  </a-button>,
                  vP(this.permissions, permissionsTechnology.Discloses.Export),
                )}

              </div>
              <div class="m-query-content">
                <span class="key-text">视频名称：</span>
                <a-input
                  placeholder="请输入查询关键字"
                  class="m-form"
                  allowClear={true}
                  value={this.queryParams.keyWord}
                  onInput={event => {
                    this.queryParams.keyWord = event.target.value;
                    this.refresh();
                  }}
                />
                <a-button
                  style="width:80px;margin-left:5px"
                  type="primary"
                  size="default"
                  onClick={() => this.refresh()}
                >
                  查询
                </a-button>
              </div>
            </a-col>
          </a-row>
        </div>
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          row-selection={{
            selectedRowKeys: this.selectedRowKeys,
            onChange: (keys, rows) => {
              this.selectedRowKeys = keys;
              this.updateSelected();
            },
          }}
          pagination={false}
          {...{
            scopedSlots: {
              xh: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              name: (text, record, index) => {
                return <a-tooltip title={text} placement="topLeft"><a onClick={() => this.view(record)}>{text}</a></a-tooltip>;
              },
              size: (text, record, index) => {
                let result = record ? FileSizeTrans(record.size) : '';
                return <a-tooltip title={result} placement="topLeft">{result}</a-tooltip>;
              },
              creationTime: (text, record, index) => {
                let result = record && record.creationTime ? moment(record.creationTime).format('YYYY-MM-DD') : '';
                return <a-tooltip title={result} placement="topLeft">{result}</a-tooltip>;
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a-button style="padding:2px" onClick={() => this.view(record, true)} type="link">
                        {' '}
                        二维码
                      </a-button>,
                      vP(this.permissions, permissionsTechnology.Discloses.Detail),
                    )}

                  </span>,

                  <span>
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsTechnology.Discloses.Detail) &&
                      vP(this.permissions, permissionsTechnology.Discloses.Update),
                    )}
                    {vIf(
                      <a-button style="padding:2px" onClick={() => this.edit(record)} type="link">
                        {' '}
                        编辑附件
                      </a-button>,
                      vP(this.permissions, permissionsTechnology.Discloses.Update),
                    )}

                  </span>,
                ];
              },
              entryTime: (text, record) => {
                return moment(record.entryTime).format('YYYY-MM-DD');
              },
              creationTime: (text, record) => {
                return moment(record.creationTime).format('YYYY-MM-DD HH:mm:ss');
              },
            },
          }}
        ></a-table>

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

        <FileSelect ref='fileSelect' axios={this.axios} onSuccess={() => this.refresh()} type={this.type} />
        <SmVideo ref='player' title='视频预览' visible={false} width={900} height={500} />
        <EditModal ref='editModal' title='附件编辑' axios={this.axios} width={700} height={400} visible={false} />
      </div>
    );
  },
};
