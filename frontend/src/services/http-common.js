import axios from "axios";

export default axios.create({
  baseURL: "http://localhost:5088/api",
  headers: {
    "Content-type": "application/json",
  },
});
