import React, { Component } from "react";
import ReviewService from "../services/review.services";
import AuthService from "../services/auth.services";

export default class AddReview extends Component {
  constructor(props) {
    super(props);
    this.onChangeTitle = this.onChangeTitle.bind(this);
    this.onChangeReview = this.onChangeReview.bind(this);
    this.onChangeRating = this.onChangeRating.bind(this);
    this.onChangeRatingReasoning = this.onChangeRatingReasoning.bind(this);
    this.onChangePlatform = this.onChangePlatform.bind(this);
    this.onChangeImageLink = this.onChangeImageLink.bind(this);
    this.saveReview = this.saveReview.bind(this);

    this.state = {
      title: "",
      review: "",
      rating: "",
      ratingReasoning: "",
      platform: "",
      image: "",
      saved: "",
    };
  }

  onChangeTitle(e) {
    this.setState({
      title: e.target.value,
    });
  }
  onChangeReview(e) {
    this.setState({
      review: e.target.value,
    });
  }
  onChangeRating(e) {
    this.setState({
      rating: e.target.value,
    });
  }
  onChangeRatingReasoning(e) {
    this.setState({
      ratingReasoning: e.target.value,
    });
  }
  onChangePlatform(e) {
    this.setState({
      platform: e.target.value,
    });
  }
  onChangeImageLink(e) {
    this.setState({
      image: e.target.value,
    });
  }

  saveReview() {
    var data = {
      idUser: AuthService.getCurrentUser().Id,
      review: this.state.review,
      title: this.state.title,
      rating: this.state.rating,
      ratingReasoning: this.state.ratingReasoning,
      platform: this.state.platform,
      image: this.state.image,
    };

    ReviewService.create(data)
      .then((res) => {
        alert("Review has been posted");
        window.location = "/";
      })
      .catch((e) => {
        console.log(e);
      });
  }

  render() {
    return (
      <div className="submit-form">
        <div className="form-group-review">
          <label htmlFor="title">Title</label>
          <input
            type="text"
            className="form-control"
            id="title"
            required
            value={this.state.title}
            onChange={this.onChangeTitle}
            name="title"
          />
        </div>
        <div className="form-group-review">
          <label htmlFor="review">Review</label>
          <textarea
            type="text"
            className="form-control"
            id="review"
            required
            value={this.state.review}
            onChange={this.onChangeReview}
            name="review"
            style={{ height: "150px" }}
          />
        </div>
        <div className="form-group-review">
          <label htmlFor="rating">Rating</label>
          <input
            type="number"
            min="1"
            max="10"
            className="form-control"
            id="rating"
            required
            value={this.state.rating}
            onChange={this.onChangeRating}
            name="rating"
          />
        </div>
        <div className="form-group-review">
          <label htmlFor="ratingReasoning">Reason for rating</label>
          <input
            type="text"
            className="form-control"
            id="ratingReasoning"
            required
            value={this.state.ratingReasoning}
            onChange={this.onChangeRatingReasoning}
            name="ratingReasoning"
          />
        </div>
        <div className="form-group-review">
          <label htmlFor="platform">Platform</label>
          <input
            type="text"
            className="form-control"
            id="platform"
            required
            value={this.state.platform}
            onChange={this.onChangePlatform}
            name="platform"
          />
        </div>
        <div className="form-group-review">
          <label htmlFor="image">Image (link)</label>
          <input
            type="text"
            className="form-control"
            id="image"
            required
            value={this.state.image}
            onChange={this.onChangeImageLink}
            name="image"
          />
        </div>
        <button
          onClick={this.saveReview}
          className="btn btn-success create-review-btn"
        >
          Submit
        </button>
      </div>
    );
  }
}
