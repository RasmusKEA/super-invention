import { Component } from "react";
import { useParams } from "react-router-dom";
import ReviewService from "../services/review.services";
import { buildStyles, CircularProgressbar } from "react-circular-progressbar";
import AuthService from "../services/auth.services";

class ReviewComponent extends Component {
  constructor(props) {
    super(props);
    this.retrieveReview = this.retrieveReview.bind(this);
    this.deleteReview = this.deleteReview.bind(this);
    this.editReview = this.editReview.bind(this);
    this.state = {
      review: {},
      isUser: false,
      isStaff: false,
      isAdmin: false,
      currentUser: undefined,
    };
  }

  componentDidMount() {
    const { id } = this.props.params;
    this.retrieveReview(id);

    const user = AuthService.getCurrentUser();
    console.log(AuthService.getCurrentUser());
    if (user) {
      this.setState({
        currentUser: user,
        isUser: user.Role.includes("ROLE_USER"),
        isStaff: user.Role.includes("ROLE_STAFF"),
        isAdmin: user.Role.includes("ROLE_ADMIN"),
      });
    }
  }

  retrieveReview(id) {
    ReviewService.get(id)
      .then((response) => {
        this.setState({
          review: response.data[0],
        });
        //console.log(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  editReview() {
    const { id } = this.props.params;
    const confirm = window.confirm("Are you sure you want edit this review?");
    if (confirm) {
      window.location = `edit/${id}`;
    }
  }

  deleteReview() {
    const { id } = this.props.params;
    const confirm = window.confirm("Are you sure you want delete this review?");
    if (confirm) {
      ReviewService.delete(id)
        .then((response) => {
          this.setState({
            review: response.data,
          });
          //console.log(response.data);
        })
        .catch((e) => {
          console.log(e);
        });
      window.location = "/";
    }
  }

  render() {
    const { review, currentUser, isAdmin, isStaff } = this.state;
    return (
      <div className="bimbom">
        <div>
          {isAdmin || (isStaff && review.IdUser === currentUser.Id) ? (
            <button
              onClick={this.editReview}
              className="btn btn-primary delete-review-btn"
            >
              Edit
            </button>
          ) : (
            ""
          )}
          {isAdmin || (isStaff && review.IdUser === currentUser.Id) ? (
            <button
              onClick={this.deleteReview}
              className="btn btn-danger delete-review-btn"
            >
              Delete
            </button>
          ) : (
            ""
          )}
        </div>
        <div className="review-container">
          {review && (
            <div className="full-review">
              <img
                src={review.image}
                style={{ width: "800px", height: "400px" }}
                alt="img"
              ></img>

              <h3 className="review-title">{review.title}</h3>
              <div className="title-div">
                <p>{review.review}</p>
              </div>
              <div className="rating-reasoning">
                <div className="rr-left">
                  <CircularProgressbar
                    value={review.rating}
                    styles={buildStyles({
                      textSize: "40px",
                      textColor: "black",
                      pathColor: "#eb3c2d",
                    })}
                    strokeWidth={8}
                    maxValue={10}
                    text={`${review.rating}`}
                  />
                </div>
                <div className="rr-right">
                  <p>{review.ratingReasoning}</p>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    );
  }
}

export default (props) => <ReviewComponent {...props} params={useParams()} />;
