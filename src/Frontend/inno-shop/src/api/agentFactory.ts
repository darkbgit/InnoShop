import axios, {
  AxiosError,
  type AxiosInstance,
  type AxiosRequestConfig,
  type AxiosResponse,
  type InternalAxiosRequestConfig,
} from "axios";

export const createAxios = (baseUrl: string): AxiosInstance => {
  const axiosInstance = axios.create({
    baseURL: baseUrl,
    headers: {
      "Content-Type": "application/json",
    },
  });

  axiosInstance.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
      const token = localStorage.getItem("jwt_token");
      if (token) config.headers!.Authorization = `Bearer ${token}`;
      return config;
    },
    (error) => Promise.reject(error)
  );

  axiosInstance.interceptors.response.use(
    async (response) => {
      return response;
    },
    (error: AxiosError) => {
      if (!error.response) {
        return Promise.reject(error);
      }
      const { data, status } = error.response as AxiosResponse;
      switch (status) {
        case 400:
          if (data && data.errors) {
            const modelStateErrors: string[] = [];
            for (const key in data.errors) {
              if (data.errors[key]) {
                modelStateErrors.push(data.errors[key]);
              }
            }
            throw modelStateErrors.flat();
          }
          break;
        case 401:
          localStorage.removeItem("jwt_token");
          break;
        default:
          break;
      }

      return Promise.reject(error);
    }
  );

  return axiosInstance;
};

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

export const requests = {
  get: <T>(
    instance: AxiosInstance,
    url: string,
    config?: AxiosRequestConfig | undefined
  ) => instance.get<T>(url, config).then(responseBody),
  post: <T>(instance: AxiosInstance, url: string, body: {}) =>
    instance.post<T>(url, body).then(responseBody),
  put: <T>(
    instance: AxiosInstance,
    url: string,
    body: {},
    config?: AxiosRequestConfig | undefined
  ) => instance.put<T>(url, body, config).then(responseBody),
  delete: <T>(instance: AxiosInstance, url: string) =>
    instance.delete<T>(url).then(responseBody),
};
