import React, { Component } from "react";
import ReviewService from "../services/review.services";
import { buildStyles, CircularProgressbar } from "react-circular-progressbar";
import { Link } from "react-router-dom";
import Moment from "moment";

export default class MiniatureList extends Component {
  constructor(props) {
    super(props);
    this.retrieveReviews = this.retrieveReviews.bind(this);

    this.state = {
      reviews: [],
    };
  }

  componentDidMount() {
    this.retrieveReviews();
  }

  retrieveReviews() {
    ReviewService.getAll()
      .then((response) => {
        this.setState({
          reviews: response.data,
        });
        //console.log(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  render() {
    const { reviews } = this.state;
    return (
      <div className="miniature-container">
        {reviews &&
          reviews.map((review, index) => (
            <Link
              to={"/review/" + review.id}
              style={{ textDecoration: "none" }}
              className="link"
              key={index}
            >
              <div className="review">
                <div className="title-date-comments">
                  <p>{review.title}</p>
                  <div className="date-comments">
                    <p>{Moment(review.createdAt).format("DD/MM/YYYY")}</p>
                  </div>
                </div>

                <div className="rating-image">
                  <div className="vl"></div>
                  <div className="rating">
                    <CircularProgressbar
                      value={review.rating}
                      styles={buildStyles({
                        textSize: "40px",
                        textColor: "black",
                        pathColor: "#eb3c2d",
                      })}
                      strokeWidth={8}
                      maxValue={10}
                      text={review.rating}
                    />
                  </div>

                  <div className="image">
                    <img
                      src={review.image}
                      alt="example"
                      style={{ width: "200px", height: "100px" }}
                    />
                  </div>
                </div>
              </div>
            </Link>
          ))}
      </div>
    );
  }
}
