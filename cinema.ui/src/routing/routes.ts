export const ROUTES = {
  private: {
    HOME: '/admin',
    MOVIE: {
      TABLE: '/admin/movies',
      ADD: '/admin/movies/add',
      DETAILS: '/admin/movies/:id',
    },
    CATEGORY: {
      TABLE: '/admin/categories',
      ADD: '/admin/categories/add',
      DETAILS: '/admin/categories/:id',
    },
    SCREENING: {
      TABLE: '/admin/screenings',
      ADD: '/admin/screenings/add',
      DETAILS: '/admin/screenings/:id',
    },
    USER: {
      TABLE: '/admin/users',
      ADD: '/admin/users/add',
      DETAILS: '/admin/users/:id',
    },
    ORDER: {
      TABLE: '/admin/orders',
    },
  },
  public: {
    HOME: '/',
  },
};
