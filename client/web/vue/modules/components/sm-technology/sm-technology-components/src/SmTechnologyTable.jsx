import { MarkType } from '../../../_utils/enum';
import { getMarkType, vIf, vP } from '../../../_utils/utils';
import permissionsSmTechnology from '../../../_permissions/sm-technology';
import { stringify } from 'querystring';
export default {
  name: 'SmTechnologyTable',
  props: {
    axios: { type: Function, default: null },
    datas: { type: Array, default: () => [] },
    type: { type: String, default: null },
    pageIndex: { type: Number, default: 1 },
    permissions: { type: Array, default: () => [] },
    maxResultCount: { type: Number, default: 10 },
    isD3: { type: Boolean, default: false },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      dataSource: [],
      loading: false,
      totalCount: 0,
      iPageIndex: 1,
      iIsD3: false, //是否三维里面的接口
      record: null,
      typeName: null,
      iMaxResultCount: 10,
      // queryParams: {
      //   jobLocationId: undefined, //施工地点
      //   professionId: undefined, //专业
      //   builderId: undefined, //土建单位
      //   markType: undefined, //检查情况
      //   maxResultCount: paginationConfig.defaultPageSize,
      // },
    };
  },
  computed: {
    d3Columns() {
      let col = [
        {
          title: '序号',
          dataIndex: 'index',
          width: 50,
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        // {
        //   title: '施工地点',
        //   dataIndex: 'jobLocation',
        //   ellipsis: true,
        //   scopedSlots: { customRender: 'jobLocation' },
        // },
        {
          title: '接口名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '专业',
          ellipsis: true,
          dataIndex: 'profession',
          scopedSlots: { customRender: 'profession' },
        },
        {
          title: '接口位置',
          ellipsis: true,
          dataIndex: 'position',
          scopedSlots: { customRender: 'position' },
        },

        {
          title: '接口编号',
          dataIndex: 'code',
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '土建接口',
          dataIndex: 'builder',
          ellipsis: true,
          scopedSlots: { customRender: 'builder' },
        },
        {
          title: '检查情况',
          ellipsis: true,
          dataIndex: 'markType',
          scopedSlots: { customRender: 'markType' },
        },
        {
          width: 140,
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];

      return col;
    },
    columns() {
      let col = [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        // {
        //   title: '施工地点',
        //   dataIndex: 'jobLocation',
        //   ellipsis: true,
        //   scopedSlots: { customRender: 'jobLocation' },
        // },
        {
          title: '接口名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '专业',
          ellipsis: true,
          dataIndex: 'profession',
          scopedSlots: { customRender: 'profession' },
        },
        {
          title: '接口位置',
          ellipsis: true,
          dataIndex: 'position',
          scopedSlots: { customRender: 'position' },
        },

        {
          title: '接口编号',
          dataIndex: 'code',
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '材料名称',
          ellipsis: true,
          dataIndex: 'marerialName',
          scopedSlots: { customRender: 'marerialName' },
        },
        {
          title: '规格',
          dataIndex: 'materialSpec',
          ellipsis: true,
          scopedSlots: { customRender: 'materialSpec' },
        },
        {
          title: '数量',
          dataIndex: 'marerialCount',
          ellipsis: true,
          scopedSlots: { customRender: 'marerialCount' },
        },
        {
          title: '土建接口',
          dataIndex: 'builder',
          ellipsis: true,
          scopedSlots: { customRender: 'builder' },
        },
        {
          title: '检查情况',
          ellipsis: true,
          dataIndex: 'markType',
          scopedSlots: { customRender: 'markType' },
        },
      ];
      if (this.typeName == 'flag' || this.typeName == 'report') {
        col.push({
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        });
      }
      return col;
    },
  },
  watch: {
    maxResultCount: {
      handler: function (val, oldVal) {
        if (this.maxResultCount) {
          this.iMaxResultCount = val;
        }
      },
      immediate: true,
      deep: true,
    },
    pageIndex: {
      handler: function (val, oldVal) {
        if (this.pageIndex) {
          this.iPageIndex = val;
        }
      },
      immediate: true,
      deep: true,
    },
    datas: {
      handler: function (val, oldVal) {
        if (this.datas) {
          this.dataSource = val;
        }
      },
      immediate: true,
      deep: true,
    },
    type: {
      handler: function (val, oldVal) {
        if (this.type) {
          this.typeName = val;
        }
      },
      immediate: true,
    },
    isD3: {
      handler: function (val, oldVal) {
        this.iIsD3 = val;
      },
      immediate: true,
    },
  },
  methods: {
    getColor(type) {
      let color;
      switch (type) {
      case MarkType.NoQualified:
        color = 'red';
        break;
      case MarkType.Qualified:
        color = 'lime';
        break;
      case MarkType.NoCheck:
        color = 'GoldEnrod';
        break;
      }
      return color;
    },
  },
  render() {
    return (
      <a-table
        columns={this.iIsD3 ? this.d3Columns : this.columns}
        rowKey={record => record.id}
        dataSource={this.dataSource}
        bordered={this.bordered}
        size={this.iIsD3 ? 'small' : 'default'}
        pagination={false}
        loading={this.loading}
        rowSelection={
          !this.iIsD3
            ? {
              onChange: (selectedRowKeys, recordList) => {
                this.$emit('rowSelection', selectedRowKeys, recordList);
              },
            }
            : null
        }
        {...{
          scopedSlots: {
            index: (text, record, index) => {
              return index + 1 + this.iMaxResultCount * (this.iPageIndex - 1);
            },
            // jobLocation: (text, record) => {
            //   let result = record.jobLocation ? record.jobLocation.name : '';
            //   return (
            //     <a-tooltip placement="topLeft" title={result}>
            //       <span>{result}</span>
            //     </a-tooltip>
            //   );
            // },
            profession: (text, record) => {
              let result = record.profession ? record.profession.name : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },
            position: (text, record) => {
              let result = record ? JSON.parse(record.position) : null;
              let _res = null;
              if (result instanceof Object) {
                let { lon, lat, alt } = result;
                lon = lon ? lon : '';
                lat = lat ? lat : '';
                alt = alt ? alt : '';
                _res = `${lon} ${lat} ${alt}`;

              } else {
                _res = record ? record.position : null;
              }
              return (
                <a-tooltip placement="topLeft" title={_res}>
                  <span>{_res}</span>
                </a-tooltip>
              );
            },
            name: (text, record) => {
              let result = record ? record.name : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },
            // count: (text, record) => {
            //   let result = record.count ? record.count : '';
            //   return (
            //     <a-tooltip placement="topLeft" title={result}>
            //       <span>{result}</span>
            //     </a-tooltip>
            //   );
            // },
            marerialName: (text, record) => {
              let result = record ? record.marerialName : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },
            materialSpec: (text, record) => {
              let result = record ? record.materialSpec : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },
            marerialCount: (text, record) => {
              let result = record ? record.marerialCount : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },
            code: (text, record) => {
              let result = record ? record.code : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },
            builder: (text, record) => {
              let result = record.builder ? record.builder.name : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },
            markType: (text, record) => {
              let result = record.markType ? getMarkType(record.markType) : '未检查';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span style={{ color: this.getColor(record.markType) }}>{result}</span>
                </a-tooltip>
              );
            },
            operations: (text, record) => {
              return this.typeName == 'flag' || this.iIsD3
                ? [
                  <span>
                    {record.markType == MarkType.NoQualified
                      ? vIf(
                        <a
                          onClick={() => {
                            this.$emit('reform', record);
                          }}
                        >
                          整改
                        </a>,
                        vP(
                          this.permissions,
                          permissionsSmTechnology.ConstructInterfaces.Reform,
                        ),
                      )
                      : vIf(
                        <a
                          onClick={() => {
                            this.$emit('sign', record);
                          }}
                        >
                          {' '}
                          标记
                        </a>,
                        vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Sign),
                      )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(
                        this.permissions,
                        record.markType == MarkType.NoQualified
                          ? permissionsSmTechnology.ConstructInterfaces.Reform
                          : permissionsSmTechnology.ConstructInterfaces.Sign,
                      ) &&
                      vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Detail) ||
                      (vP(
                        this.permissions,
                        record.markType == MarkType.NoQualified
                          ? permissionsSmTechnology.ConstructInterfaces.Reform
                          : permissionsSmTechnology.ConstructInterfaces.Sign,
                      ) && this.iIsD3),
                    )}
                    {vIf(
                      <a
                        disabled={
                          record.constructInterfaceInfos.length > 0 ||
                            !record.constructInterfaceInfos
                            ? false
                            : true
                        }
                        onClick={() => {
                          this.$emit('view', record);
                        }}
                      >
                        详情
                      </a>,
                      vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Detail),
                    )}
                    {this.iIsD3
                      ? [
                        vIf(
                          <a-divider type="vertical" />,
                          (vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Detail) && this.iIsD3),
                        ),
                        <a
                          onClick={() => {
                            if (record ) {
                              this.$emit('flyTo', {
                                groupName: record.equipment && record.equipment.group
                                  ? record.equipment.group.name
                                  : '',
                                name: record.name,
                                state: record.markType,
                                position: record ? record.position : '',
                              });
                            }
                          }}
                        >
                          定位
                        </a>,
                      ]
                      : []}
                  </span>,
                ]
                : this.typeName == 'report'
                  ? [
                    <span>
                      {vIf(
                        <a
                          disabled={
                            record.constructInterfaceInfos.length > 0 ||
                              !record.constructInterfaceInfos
                              ? false
                              : true
                          }
                          onClick={() => {
                            this.$emit('view', record);
                          }}
                        >
                          记录
                        </a>,
                        vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Detail),
                      )}
                      {vIf(
                        <a-divider type="vertical" />,
                        vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Export) &&
                        vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Detail),
                      )}
                      {vIf(
                        <a
                          onClick={() => {
                            this.$emit('exportReport', record);
                          }}
                        >
                          导出
                        </a>,
                        vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Export),
                      )}
                    </span>,
                  ]
                  : [];
            },
          },
        }}
      ></a-table>
    );
  },
};
