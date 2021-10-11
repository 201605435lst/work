<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <!-- <a-radio-group v-model="pageState" @change="onPageStateChange">
      <a-radio-button value="add">
        Add
      </a-radio-button>
      <a-radio-button value="edit">
        Edit
      </a-radio-button>
      <a-radio-button value="view">
        View
      </a-radio-button>
    </a-radio-group> -->
    <sm-cr-plan-maintenance-record
      :axios="axios"
      :organizationId="organizationId"
      :equipmentId="equipmentId"
      :repairGroupId="repairGroupId"
      :equipType="equipType"
      :equipName="equipName"
      :equipModelNumber="equipModelNumber"
      :equipModelCode="equipModelCode"
      :installationSite="installationSite"
      :time="time"
      :workOrderIds="workOrderIds"
      repairTagKey="RepairTag.RailwayWired"
      @ok="onOk"
      @cancel="onCancel"/>
  </div>

</template>
<!--RepairTag.RailwayHighSpeed-->
<!--RepairTag.RailwayWired-->
<script>
import axios from '@/utils/axios.js'
import moment from 'moment';

export default {
  data(){
    return {
      axios,
      organizationId:'39f8757d-ecc8-f872-3314-1a955bbdeb12',//'74e2fdd1-51c8-4bf0-88ca-31f8c37f3a0a',
      equipmentId:null,//'e27bd851-6655-4dfd-9e46-20286b2218f9',
      repairGroupId:'e8b28c25-20ef-4562-ac9e-593b85d5121c',//'2aa88e32-4a99-5ed9-f5d9-39f6d3b1dce3',
      equipType: "OTN设备",
      equipName: "传输网",
      equipModelNumber: "未关联设备",
      equipModelCode: null,
      // maintenanceOrg: "选择的维护单位",
      installationSite: null,
      time:moment().format('YYYY-MM-DD'),
      workOrderIds: ["14b67ce4-116b-4b8b-b51a-52c6a33d4f83", "14b67ce4-116b-4b8b-b51a-52c6a33d4f83", "14b67ce4-116b-4b8b-b51a-52c6a33d4f83", "37cae994-0d87-48a1-b38a-c9766e37bb7c", "37cae994-0d87-48a1-b38a-c9766e37bb7c", "37cae994-0d87-48a1-b38a-c9766e37bb7c", "37cae994-0d87-48a1-b38a-c9766e37bb7c", "37cae994-0d87-48a1-b38a-c9766e37bb7c", "37cae994-0d87-48a1-b38a-c9766e37bb7c", "37cae994-0d87-48a1-b38a-c9766e37bb7c", "2c2605a0-48b8-4f15-9053-f01683ce8473", "2c2605a0-48b8-4f15-9053-f01683ce8473", "2c2605a0-48b8-4f15-9053-f01683ce8473", "a3e4b1fc-a45e-4e93-9e36-cf699eb4c9f3", "de14d379-ad77-4d78-b3e9-823aac9b78f5", "5e52a4ee-331c-45a8-a94d-3fb88a419cf4", "4e1bcedc-bd1c-4736-a654-f4915f65af57", "8a0bdc3b-fd30-43c8-969f-38aabf0987b3", "f99c83d6-85f6-4241-8bb3-8c4a7e91fb83", "8681ffc3-2fd9-414e-ab2a-e780c42519b6", "fc1ec84e-bf52-4619-831e-12363217caab", "fc1ec84e-bf52-4619-831e-12363217caab", "b4af6927-4027-4f01-9676-fe740396c7e6", "b4af6927-4027-4f01-9676-fe740396c7e6", "6fc5b291-6c93-4e53-8385-9a496562dce4", "6fc5b291-6c93-4e53-8385-9a496562dce4", "690112b8-915e-4d2b-b53b-b17dadf3a080", "690112b8-915e-4d2b-b53b-b17dadf3a080", "f857faa6-e89b-48bf-8cc2-819620dc2732", "c3f3fb1e-7b9d-4443-b6e1-b0247190a34e", "c3f3fb1e-7b9d-4443-b6e1-b0247190a34e", "5fd74781-e094-4c29-8cd3-e2a524085cfb", "92341e9a-eb0c-4b2f-a5e5-3770d653219c", "92341e9a-eb0c-4b2f-a5e5-3770d653219c", "5186b0f6-3aef-4e0b-a657-9415127b6d7e", "5186b0f6-3aef-4e0b-a657-9415127b6d7e", "40ad12ff-5c30-40e1-9092-6b23c5da55ad", "40ad12ff-5c30-40e1-9092-6b23c5da55ad", "2839c345-7b6d-4731-af49-14bcf7e91941", "2839c345-7b6d-4731-af49-14bcf7e91941", "56680fd8-dc02-422f-aace-2d5f87c0d808", "56680fd8-dc02-422f-aace-2d5f87c0d808", "408aad6c-85ce-4772-b60e-bdd1f90cb98d", "408aad6c-85ce-4772-b60e-bdd1f90cb98d", "a16cdac2-0fc5-480f-9a92-92217fb9b865", "56799b1f-912f-46a6-b097-ef216f93b967", "ae21420c-688e-4ca6-a510-de26b73e9d52", "7043fc93-0d02-48c5-bd65-6fc9fc638a92", "3cc8d6e4-b1bd-47ef-8b2f-c5dc498fd366", "36b3d129-aa8c-441c-bd11-5fc8fe93ab8f", "2b0f52f8-9953-46ba-9d3a-2a8c9e698496", "294d9310-3dd5-4220-a7f4-8ad724b5fa75", "e19cfa36-a2ac-4ffb-b194-8d342e1e4c73", "ef1457c0-8cc2-4856-98f4-09e6713fa198", "ef1457c0-8cc2-4856-98f4-09e6713fa198", "f857faa6-e89b-48bf-8cc2-819620dc2732", "a7f794a6-b606-4743-85fa-9df33294529e", "5fd74781-e094-4c29-8cd3-e2a524085cfb", "fa21fe6e-e9b2-4aa6-abf1-1bd46c343a98", "b56bf99f-7ca3-482d-a946-3483926ee877", "b56bf99f-7ca3-482d-a946-3483926ee877", "3213bfd0-4597-40ce-addd-614a2d835bd1", "e30b28f0-9cbe-4841-ac4a-e8ac516473f0", "e30b28f0-9cbe-4841-ac4a-e8ac516473f0", "e30b28f0-9cbe-4841-ac4a-e8ac516473f0", "e30b28f0-9cbe-4841-ac4a-e8ac516473f0", "e30b28f0-9cbe-4841-ac4a-e8ac516473f0", "e30b28f0-9cbe-4841-ac4a-e8ac516473f0", "a19f311a-9921-4e95-bc77-b30c89edbd6d", "22d2902c-899c-43e4-bec3-6add8ad808b5", "593bce9f-2e83-470f-b95c-9855cd5a21da", "1a514fcd-f25f-401e-a469-db12cc915ab4", "e382c3f6-fdac-4f3b-bf18-92fdb582bc39", "4732f1d9-e313-4252-9600-de250ef1ec9a", "c0e0d565-6ca3-4f6b-a488-d4d02e5c13ce", "7374b357-9b96-44e2-9b59-b48431018820", "a6d1f2f2-0e48-4af7-9f17-df43a22f0ca0", "7374b357-9b96-44e2-9b59-b48431018820", "561c507a-3aa3-4125-998e-f255558db150", "cccf426e-3364-4fa0-a435-50e2f55beb2b", "cccf426e-3364-4fa0-a435-50e2f55beb2b", "bca262ea-7206-45b6-b10e-9b74c05d6814", "ebfc2ab9-4f2d-4bb4-892d-f53089afbe2e", "6a2f118e-2055-4166-8f6a-2ba59b6940a0", "40aa80d9-7fe9-4a4e-910f-66f4fa330bce", "2c2605a0-48b8-4f15-9053-f01683ce8473", "1115e7e8-00ef-4e69-ad13-4998d3030aaa", "1115e7e8-00ef-4e69-ad13-4998d3030aaa", "1115e7e8-00ef-4e69-ad13-4998d3030aaa", "1115e7e8-00ef-4e69-ad13-4998d3030aaa", "1115e7e8-00ef-4e69-ad13-4998d3030aaa", "2c2605a0-48b8-4f15-9053-f01683ce8473", "2c2605a0-48b8-4f15-9053-f01683ce8473", "6a2f118e-2055-4166-8f6a-2ba59b6940a0", "6a2f118e-2055-4166-8f6a-2ba59b6940a0", "6a2f118e-2055-4166-8f6a-2ba59b6940a0", "6a2f118e-2055-4166-8f6a-2ba59b6940a0", "6a2f118e-2055-4166-8f6a-2ba59b6940a0", "6a2f118e-2055-4166-8f6a-2ba59b6940a0", "5b07b521-97fd-4fa4-93d7-ca4110529150"]
    }
  },
  created(){
  },
  methods:{
    onOk(){
      this.$message.info(`onSuccess`)
    },
    onCancel(){
      this.$message.info(`onCancel`)
    },
  }
}
</script>
```
