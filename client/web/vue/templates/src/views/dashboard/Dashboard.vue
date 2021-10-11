<template>
  <a-card style="background-color: #ececec;">
    <a-row :gutter="24">
      <a-col :sm="24" :md="9">
        <SmDashboardListCard
          :axios="axios"
          :permissions="permissions"
          style="margin-bottom:24px"
          action="/api/app/cmsArticle/getList"
          :columns="columns"
          title="要闻"
          theme="default"
          category-code="news"
          @click="onClick"
          @more="more"
        >
          <template slot="icon">
            <a-icon type="dribbble" />
          </template>
          <template slot="extra">
            <a @click="more('news')">更多</a>
          </template>
        </SmDashboardListCard>
      </a-col>
      <a-col :sm="24" :md="9">
        <SmDashboardListCard
          :axios="axios"
          :permissions="permissions"
          style="margin-bottom:24px"
          action="/api/app/cmsArticle/getList"
          title="资讯"
          :columns="columns"
          theme="default.thumb"
          category-code="informations"
          @click="onClick"
          @more="more"
        >
          <template slot="icon">
            <a-icon type="weibo" />
          </template>
          <template slot="extra">
            <a @click="more('informations')">更多</a>
          </template>
        </SmDashboardListCard>
      </a-col>

      <a-col :sm="24" :md="6">
        <SmDashboardListCard
          :axios="axios"
          :permissions="permissions"
          style="margin-bottom:24px"
          action="/api/app/cmsArticle/getList"
          title="通知"
          :columns="columnsOfMessage"
          theme="simple"
          category-code="message"
          @click="onClick"
          @more="more"
        >
          <template slot="icon">
            <a-icon type="message" />
          </template>
          <template slot="extra">
            <a @click="more('message')">更多</a>
          </template>
        </SmDashboardListCard>
      </a-col>
      <a-col :sm="24" :md="9">
        <SmDashboardListCard
          :axios="axios"
          :permissions="permissions"
          action="/api/app/cmsArticle/getList"
          title="今日话题"
          :columns="columns"
          theme="default"
          category-code="topics"
          @click="onClick"
          @more="more"
        >
          <template slot="icon">
            <a-icon type="sound" />
          </template>
          <template slot="extra">
            <a @click="more('topics')">更多</a>
          </template>
        </SmDashboardListCard>
      </a-col>
      <a-col :sm="24" :md="9">
        <SmDashboardListCard
          :axios="axios"
          :permissions="permissions"
          action="/api/app/cmsArticle/getList"
          title="视觉焦点"
          :columns="columns"
          theme="default.thumb"
          category-code="focus"
          @click="onClick"
          @more="more"
        >
          <template slot="icon">
            <a-icon type="video-camera" />
          </template>
          <template slot="extra">
            <a @click="more('focus')">更多</a>
          </template>
        </SmDashboardListCard>
      </a-col>

      <a-col :sm="24" :md="6">
        <SmDashboardListCard
          :axios="axios"
          :permissions="permissions"
          action="/api/app/cmsArticle/getList"
          title="证券"
          :columns="columnsOfMessage"
          theme="simple"
          category-code="security"
          @click="onClick"
          @more="more"
        >
          <template slot="icon">
            <a-icon type="transaction" />
          </template>
          <template slot="extra">
            <a @click="more('security')">更多</a>
          </template>
        </SmDashboardListCard>
      </a-col>
    </a-row>
    <GlobalFooter />
  </a-card>
</template>
<script>
import SmDashboardListCard from 'snweb-module/es/sm-dashboard/sm-dashboard-list-card';
import { mapGetters } from 'vuex';
import moment from 'moment';
import GlobalFooter from '@/components/GlobalFooter';

export default {
  name: 'Dashboard',
  components: { SmDashboardListCard, GlobalFooter },
  computed: {
    ...mapGetters(['permissions']),
    columns() {
      return [
        {
          title: '文章标题',
          dataIndex: 'title',
          ellipsis: true,
          width: 100,
        },
        {
          title: '创建时间',
          dataIndex: 'date',
          customRender: (text, record, index) => {
            return moment(text).format('YYYY-MM-DD HH:mm:ss');
          },
          width: 120,
          ellipsis: true,
        },
        {
          title: '详情',
          dataIndex: 'summary',
          ellipsis: true,
        },
      ];
    },

    columnsOfMessage() {
      return [
        {
          title: '文章标题',
          dataIndex: 'title',
          ellipsis: true,
        },
        {
          title: '创建时间',
          dataIndex: 'date',
          customRender: (text, record, index) => {
            return moment(text).format('YYYY-MM-DD HH:mm:ss');
          },
          width: 140,
          ellipsis: true,
          align: 'right',
        },
      ];
    },
  },
  methods: {
    onClick(event, index, id, data) {
      console.log(event, index, id, data);
      this.$router.push({
        name: 'article-preview',
        params: {
          id,
        },
      });
    },
    more(categoryCode) {
      console.log(categoryCode);
      this.$router.push({
        name: 'category-preview',
        params: {
          categoryCode,
        },
      });
    },
  },
};
</script>
