import http from "./http-common";

class CommentService {
  getAll(id) {
    return http.get(`/comment/all?idReview=${id}`);
  }

  get(id) {
    return http.get(`/comment/${id}`);
  }

  create(data) {
    return http.post("/comment", data);
  }

  update(id, data) {
    return http.put(`/comment/${id}`, data);
  }

  delete(id) {
    return http.delete(`/comment/${id}`);
  }
}

export default new CommentService();
