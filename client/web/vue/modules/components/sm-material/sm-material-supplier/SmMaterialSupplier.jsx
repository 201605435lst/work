import ApiSupplier from '../../sm-api/sm-material/Supplier';
import * as utils from '../../_utils/utils';
import {
  requestIsSuccess,
  getSupplierLevel,
  getSupplierType,
  getSupplierProperty,
} from '../../_utils/utils';
import moment from 'moment';
import { SupplierLevel, SupplierType, SupplierProperty, PageState } from '../../_utils/enum';
// import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import './style/index.less';

let apiSupplier = new ApiSupplier();
const formFields = [
  'name',
  'type',
  'level',
  'property',
  'principal',
  'telephone',
  'legalPerson',
  'tin',
  'businessScope',
  'openingBank',
  'bankAccount',
  'accountOpeningUnit',
  'registeredAssets',
  'address',
  'code',
  'remark',
  // 'supplierRltAccessories',
];
export default {
  name: 'SmMaterialSupplier',
  props: {
    axios: { type: Function, default: null },
    pageState: { type: String, default: PageState.Add }, // 页面状态
    id: { type: String, default: null }, // 数据id
  },
  data() {
    return {
      iId: null,
      form: this.$form.createForm(this, {}),
      supplier: null,
      contacts: [], //联系人列表
      loading: false,
      editable: false,
      selectedContacts: [], //已选联系人ids
      supplierRltAccessories:[],//附件
    };
  },

  computed: {
    columns() {
      return [
        {
          title: '联系人',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '手机',
          dataIndex: 'telephone',
          ellipsis: true,
          scopedSlots: { customRender: 'telephone' },
        },
        {
          title: '电话',
          dataIndex: 'landlinePhone',
          scopedSlots: { customRender: 'landlinePhone' },
        },
        {
          title: '邮箱',
          dataIndex: 'email',
          ellipsis: true,
          scopedSlots: { customRender: 'email' },
        },
        {
          title: 'QQ',
          dataIndex: 'qq',
          ellipsis: true,
          scopedSlots: { customRender: 'qq' },
        },
        this.pageState != PageState.View
          ? {
            title: '操作',
            dataIndex: 'operations',
            width: '140px',
            scopedSlots: { customRender: 'operations' },
          }
          : {
            width: 1,
          },
      ];
    },
  },

  watch: {
    id: {
      handler: function(value, oldValue) {
        this.iId = this.id;
        if (value) {
          this.initAxios();
          this.refresh();
        } else {
          this.contacts = [];
          this.form.resetFields();
        }
      },
      immediate: true,
    },
    pageState: {
      handler: function(value, oldValue) {
        this.iId = this.id;
        if (value !== PageState.Add) {
          this.initAxios();
          this.refresh();
        } else {
          this.contacts = [];
          this.form.resetFields();
        }
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiSupplier = new ApiSupplier(this.axios);
    },

    //初始化计划列表
    async refresh(id) {
      if (id) {
        this.iId = id;
      }
      if (!this.pageState || this.pageState === PageState.Add) {
        return;
      }
      let response = await apiSupplier.get(this.iId);
      if (requestIsSuccess(response) && response.data) {
        this.supplier = response.data;
        this.supplierRltAccessories = this.supplier.supplierRltAccessories
          ? this.supplier.supplierRltAccessories.map(item => item.file)
          : [];
        this.contacts = this.supplier.supplierRltContacts
          ? this.supplier.supplierRltContacts.map((item, index) => {
            return {
              ...item,
              index: index + 1,
              editable: false,
            };
          })
          : [];
        this.$nextTick(() => {
          let values = utils.objFilterProps(this.supplier, formFields);
          this.form.setFieldsValue(values);
        });
      }
    },

    close() {
      this.$emit('cancel');
      this.contacts = [];
      this.supplierRltAccessories=[];
      this.form.resetFields();
    },

    add() {
      this.contacts.push({
        index: this.contacts.length + 1,
        name: '',
        telephone: '',
        landlinePhone: '',
        email: '',
        qq: '',
        editable: true,
      });
    },

    remove(indexs) {
      this.contacts = this.contacts.filter(x => indexs.indexOf(x.index) === -1);
      this.selectedContacts = [];
    },
    /* 判断是否有空数据 */
    isEmptyData(data){
     
    },
    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if(this.contacts && this.contacts.some(item=>!(item.name && (item.telephone ||item.landlinePhone)))){
            this.$message.warning('联系人和联系方式不能为空');
          }
          else{
            let data = JSON.parse(JSON.stringify(values));
            console.log(data);
            await this.$refs.fileUpload.commit();
            data.supplierRltAccessories =
              this.supplierRltAccessories.length > 0
                ? this.supplierRltAccessories.map(item => {
                  return {
                    fileId: item.id,
                  };
                })
                : [];
            data.supplierRltContacts =
              this.contacts.length > 0
                ? this.contacts.map(item => {
                  return {
                    name: item.name,
                    telephone: item.telephone,
                    landlinePhone: item.landlinePhone,
                    qq: item.qq,
                    email: item.email,
                  };
                })
                : [];
            this.loading = true;
            let response = null;
            if (this.pageState === PageState.Add) {
              response = await apiSupplier.create(data);
            } else if (this.pageState === PageState.Edit) {
              response = await apiSupplier.update({ ...data, id: this.supplier.id });
            }
  
            if (response && requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.$emit('ok');
              this.contacts = [];
              this.supplierRltAccessories=[];
              this.form.resetFields();
            }
          
            this.loading = false;
          }
        }
      });
    },
  },
  render() {
    let supplierTypeOptions = [];
    for (let item in SupplierType) {
      supplierTypeOptions.push(
        <a-select-option key={SupplierType[item]}>
          {getSupplierType(SupplierType[item])}
        </a-select-option>,
      );
    }

    let supplierLevelOptions = [];
    for (let item in SupplierLevel) {
      supplierLevelOptions.push(
        <a-select-option key={SupplierLevel[item]}>
          {getSupplierLevel(SupplierLevel[item])}
        </a-select-option>,
      );
    }

    let supplierPropertyOptions = [];
    for (let item in SupplierProperty) {
      supplierPropertyOptions.push(
        <a-select-option key={SupplierProperty[item]}>
          {getSupplierProperty(SupplierProperty[item])}
        </a-select-option>,
      );
    }
    return (
      <div class="sm-material-supplier">
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={24} md={8}>
              <a-form-item label="供应商名称" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  disabled={this.pageState == PageState.View}
                  placeholder={this.pageState == PageState.View ? '' : '请输入供应商名称'}
                  v-decorator={[
                    'name',
                    {
                      initialValue: '',
                      rules: [{ required: true, message: '请输入供应商名称' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="供应商类型" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-select
                  disabled={this.pageState == PageState.View}
                  placeholder={this.pageState == PageState.View ? '' : '请选择供应商类型'}
                  v-decorator={[
                    'type',
                    {
                      initialValue: SupplierType.Supplier,
                      rules: [{ required: true, message: '请选择供应商类型' }],
                    },
                  ]}
                >
                  {supplierTypeOptions}
                </a-select>
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="供应商等级" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-select
                  disabled={this.pageState == PageState.View}
                  placeholder={this.pageState == PageState.View ? '' : '请选择供应商等级'}
                  v-decorator={[
                    'level',
                    {
                      initialValue: SupplierLevel.LevelI,
                      rules: [{ required: true, message: '请选择供应商等级' }],
                    },
                  ]}
                >
                  {supplierLevelOptions}
                </a-select>
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="供应商性质" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-select
                  disabled={this.pageState == PageState.View}
                  placeholder={this.pageState == PageState.View ? '' : '请选择供应商性质'}
                  v-decorator={[
                    'property',
                    {
                      initialValue: SupplierProperty.Unit,
                      rules: [{ required: true, message: '请选择供应商性质' }],
                    },
                  ]}
                >
                  {supplierPropertyOptions}
                </a-select>
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="负责人" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  disabled={this.pageState == PageState.View}
                  placeholder={this.pageState == PageState.View ? '' : '请输入负责人'}
                  v-decorator={[
                    'principal',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="联系电话" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入联系电话'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'telephone',
                    {
                      initialValue: '',
                      // rules: [
                      //   {
                      //     whitespace: true,
                      //   },
                      //   {
                      //     pattern: /^1[3456789]d{9}$/,
                      //     message: "手机号格式错误",
                      //   },
                      // ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="法人代表" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入法人代表'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'legalPerson',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="纳税人识别号" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入纳税人识别号'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'tin',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="经营范围" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入经营范围'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'businessScope',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="开户行名称" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入开户行名称'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'openingBank',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="开户行账户" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入开户行账户'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'bankAccount',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="开户单位" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入开户单位'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'accountOpeningUnit',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="注册资本" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入注册资本'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'registeredAssets',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="通信地址" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入地址'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'address',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={8}>
              <a-form-item label="供应商编号" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '供应商编号'}
                  disabled
                  v-decorator={[
                    'code',
                    {
                      initialValue: '自动生成',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="备注" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-textarea
                  axios={this.axios}
                  placeholder={this.pageState == PageState.View ? '' : '请输入备注'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'remark',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="附件" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <SmFileUpload
                  ref="fileUpload"
                  disabled={this.pageState == PageState.View}
                  mode={this.pageState == PageState.View ? "view" : "edit"}
                  axios={this.axios}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.pageState == PageState.View ? '' : '请选择附件'}
                  onSelected={(item) => {
                    this.supplierRltAccessories = item;
                  }}
                  fileList={this.supplierRltAccessories}
                />

                {/* <SmFileManageSelect
                  disabled={this.pageState == PageState.View}
                  axios={this.axios}
                  height={73}
                  multiple={true}
                  placeholder={this.pageState == PageState.View ? '' : '请选择'}
                  enableDownload={true}
                  v-decorator={[
                    '',
                    {
                      initialValue: [],
                    },
                  ]}
                /> */}
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>

        <a-tabs default-active-key="1">
          <a-tab-pane key="1" tab="联系人">
            {this.pageState === PageState.View ? (
              undefined
            ) : (
              <div style="margin-bottom:15px;">
                <a-button type="primary" style="margin-right:10px;" onClick={this.add}>
                  新增
                </a-button>
                <a-button
                  style="background-color:#ff5f5f; color:#fff"
                  onClick={() => this.remove(this.selectedContacts)}
                >
                  删除
                </a-button>
              </div>
            )}
            <a-table
              columns={this.columns}
              dataSource={this.contacts}
              rowKey={record => record.index}
              bordered={this.bordered}
              loading={this.loading}
              pagination={false}
              rowSelection={
                this.pageState !== PageState.View
                  ? {
                    columnWidth: 30,
                    selectedRowKeys: this.selectedContacts,
                    onChange: selectedRowKeys => {
                      this.selectedContacts = selectedRowKeys;
                    },
                  }
                  : null
              }
              {...{
                scopedSlots: {
                  name: (text, record, index) => {
                    return record.editable ? (
                      <a-input
                        placeholder={this.pageState == PageState.View ? '' : '请输入姓名'}
                        value={record.name}
                        onChange={event => {
                          record.name = event.target.value;
                        }}
                      />
                    ) : (
                      record.name
                    );
                  },

                  telephone: (text, record, index) => {
                    return record.editable ? (
                      <a-input
                        placeholder={this.pageState == PageState.View ? '' : '请输入手机号'}
                        value={record.telephone}
                        onChange={event => {
                          record.telephone = event.target.value;
                        }}
                      />
                    ) : (
                      record.telephone
                    );
                  },

                  landlinePhone: (text, record, index) => {
                    return record.editable ? (
                      <a-input
                        placeholder={this.pageState == PageState.View ? '' : '请输入电话号'}
                        value={record.landlinePhone}
                        onChange={event => {
                          record.landlinePhone = event.target.value;
                        }}
                      />
                    ) : (
                      record.landlinePhone
                    );
                  },

                  email: (text, record, index) => {
                    return record.editable ? (
                      <a-input
                        placeholder={this.pageState == PageState.View ? '' : '请输入邮箱'}
                        value={record.email}
                        onChange={event => {
                          record.email = event.target.value;
                        }}
                      />
                    ) : (
                      record.email
                    );
                  },

                  qq: (text, record, index) => {
                    return record.editable ? (
                      <a-input
                        placeholder={this.pageState == PageState.View ? '' : '请输入QQ号'}
                        value={record.qq}
                        onChange={event => {
                          record.qq = event.target.value;
                        }}
                      />
                    ) : (
                      record.qq
                    );
                  },

                  operations: (text, record) => {
                    return (
                      <span>
                        {!record.editable
                          ? [
                            <a
                              onClick={() => {
                                record.editable = true;
                              }}
                            >
                                编辑
                            </a>,
                            <a-divider type="vertical" />,
                            <a
                              style="color:red;"
                              onClick={() => {
                                this.remove([record.index]);
                              }}
                            >
                                删除
                            </a>,
                          ]
                          : [
                            <a
                              onClick={() => {
                                record.editable = false;
                              }}
                            >
                                保存
                            </a>,
                            <a-divider type="vertical" />,
                            <a
                              style="color:red;"
                              onClick={() => {
                                this.remove([record.index]);
                              }}
                            >
                                删除
                            </a>,
                          ]}
                      </span>
                    );
                  },
                },
              }}
            ></a-table>
          </a-tab-pane>
        </a-tabs>

        <div style="float: right; margin-top:20px;">
          {this.pageState == PageState.View ? (
            <a-button
              onClick={() => {
                this.close();
              }}
            >
              关闭
            </a-button>
          ) : (
            [
              <a-button
                type="primary"
                disabled={this.pageState == PageState.View}
                style="margin-right: 20px"
                onClick={this.save}
                loading={this.loading}
              >
                保存
              </a-button>,
              <a-button
                onClick={() => {
                  this.close();
                }}
              >
                取消
              </a-button>,
            ]
          )}
        </div>
      </div>
    );
  },
};
