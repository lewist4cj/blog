import { createRouter, createWebHistory } from 'vue-router'
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
import { useUserStore } from '@/stores/user'
import { ElMessage } from 'element-plus'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: 'web',
      path: '/',
      component: () => import('@/views/web/Layout.vue'),
      children: [
        { name: 'home', path: '', component: () => import('@/views/web/Home.vue') },
        { name: 'articleDetail', path: 'article/:id', component: () => import('@/views/web/ArticleDetail.vue') },
        {
          name: 'userCenter',
          path: 'center',
          component: () => import('@/views/web/user/UserLayout.vue'),
          meta: { title: '用户中心', requiresAuth: true },
          children: [
            { name: 'userInfo', path: 'info', component: () => import('@/views/web/user/Info.vue') },
            { name: 'userAccount', path: 'account', component: () => import('@/views/web/user/Account.vue') },
            { name: 'userHistory', path: 'history', component: () => import('@/views/web/user/History.vue') },
          ],
        },
        {
          name: 'messages',
          path: 'messages',
          component: () => import('@/views/web/Messages.vue'),
          meta: { title: '消息通知', requiresAuth: true },
        },
        {
          name: 'userProfile',
          path: 'user/:id',
          component: () => import('@/views/web/user/ProfileLayout.vue'),
          children: [
            { name: 'userArticle', path: 'article', component: () => import('@/views/web/user/ArticleList.vue') },
            { name: 'userCollect', path: 'collect', component: () => import('@/views/web/user/CollectList.vue') },
          ],
        },
        {
          name: 'platform',
          path: 'platform',
          component: () => import('@/views/web/platform/Layout.vue'),
          meta: { title: '文章管理', requiresAuth: true },
          children: [
            {
              name: 'platformArticle',
              path: 'article',
              component: () => import('@/views/web/platform/ArticleList.vue'),
            },
            {
              name: 'platformArticleAdd',
              path: 'article/add',
              component: () => import('@/views/web/platform/ArticleEdit.vue'),
            },
            {
              name: 'platformArticleEdit',
              path: 'article/:id/edit',
              component: () => import('@/views/web/platform/ArticleEdit.vue'),
            },
          ],
        },
      ],
    },
    {
      name: 'login',
      path: '/login',
      component: () => import('@/views/login/Login.vue'),
    },
    {
      name: 'admin',
      path: '/admin',
      component: () => import('@/views/admin/Layout.vue'),
      meta: { title: '管理后台', requiresAuth: true, role: [1, 3] },
      children: [
        {
          name: 'dashboard',
          path: '',
          component: () => import('@/views/admin/Dashboard.vue'),
          meta: { title: '数据统计' },
        },
        {
          name: 'userManage',
          path: 'users',
          component: () => import('@/views/admin/UserManage.vue'),
          meta: { title: '用户管理', role: [1] },
        },
        {
          name: 'articleManage',
          path: 'articles',
          component: () => import('@/views/admin/ArticleManage.vue'),
          meta: { title: '文章管理' },
        },
        {
          name: 'siteSettings',
          path: 'settings',
          component: () => import('@/views/admin/SiteSettings.vue'),
          meta: { title: '站点配置', role: [1] },
        },
      ],
    },
    { name: 'notfound', path: '/:pathMatch(.*)*', component: () => import('@/views/web/NotFound.vue') },
  ],
})

router.beforeEach((to, _from) => {
  const store = useUserStore()
  if (to.meta.title) document.title = to.meta.title as string

  if (to.meta.requiresAuth || to.meta.role) {
    if (!store.isLogin) {
      ElMessage.warning('请先登录')
      return { name: 'login', query: { redirect: to.fullPath } }
    }
    if (to.meta.role && Array.isArray(to.meta.role) && !to.meta.role.includes(store.userInfo.role)) {
      ElMessage.warning('权限不足')
      return { name: 'home' }
    }
  }
  NProgress.start()
  return true
})

router.afterEach(() => NProgress.done())

export default router
