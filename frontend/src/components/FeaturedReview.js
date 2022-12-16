import React, { Component } from "react";
import ReviewService from "../services/review.services";
import { buildStyles, CircularProgressbar } from "react-circular-progressbar";
import { Link } from "react-router-dom";

export default class FeaturedReview extends Component {
  constructor(props) {
    super(props);
    this.retrieveFeatured = this.retrieveFeatured.bind(this);
    this.state = {
      featured: [],
    };
  }

  componentDidMount() {
    this.retrieveFeatured();
  }

  retrieveFeatured() {
    ReviewService.getFeatured()
      .then((response) => {
        this.setState({
          featured: response.data,
        });
        //console.log(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  render() {
    const { featured } = this.state;
    return (
      <div className="featuredReview">
        {featured &&
          featured.map((feat, index) => (
            <div className="outer-featured">
              <Link
                to={"/review/" + feat.id}
                style={{ textDecoration: "none" }}
                className="featured-link"
                key={index}
              >
                <div className="featuredReview">
                  <div className="featured-image">
                    <img
                      src={feat.image}
                      alt="example"
                      className="featured-image-tag"
                    />
                  </div>

                  <div className="review-snippet">
                    <div className="featured-rating">
                      <CircularProgressbar
                        value={feat.rating}
                        styles={buildStyles({
                          textSize: "40px",
                          textColor: "black",
                          pathColor: "#eb3c2d",
                        })}
                        strokeWidth={8}
                        maxValue={10}
                        text={`${feat.rating}`}
                      />
                    </div>
                    <div className="featured-text">
                      <div className="featured-title">
                        <p>{feat.title}</p>
                      </div>
                      <div className="under-title">
                        <p>{feat.title}</p>
                      </div>
                    </div>
                  </div>
                </div>
              </Link>
            </div>
          ))}
      </div>
    );
  }
}
