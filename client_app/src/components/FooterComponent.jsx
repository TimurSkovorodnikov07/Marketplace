import githubIcon from "../files/images/githubIcon.png";

export function FooterComponent({}) {
  return (
    <footer className="app-footer">
      <div className="app-footer-content">
        <div className="about">
          <h2>About</h2>
          Blah blah blah blah blah blah blah blah blah blah blah blah blah blah
          blah blah blah blah blah blah blah blah blah blah blah blah blah blah
          blah blah blah blah blah blah blah blah blah blah...
        </div>

        <div className="social-media">
          <h2>Social media</h2>
          <ul className="social-media-list">
            <li className="media-icon">
              <a href="https://github.com/TimurSkovorodnikov07">
                <img src={githubIcon} />
              </a>
            </li>
            <li className="media-icon">
              <a href="https://github.com/TimurSkovorodnikov07">
                <img src={githubIcon} />
              </a>
            </li>

            <li className="media-icon">
              <a href="https://github.com/TimurSkovorodnikov07">
                <img src={githubIcon} />
              </a>
            </li>
            <li className="media-icon">
              <a href="https://github.com/TimurSkovorodnikov07">
                <img src={githubIcon} />
              </a>
            </li>
          </ul>
        </div>

        <ul className="contact-info">
          <h2>Contact</h2>
          <li className="contact-info-element">123 street</li>
          <li className="contact-info-element">+0 000 000 00 00</li>
          <li className="contact-info-element">example@mail.ex</li>
        </ul>
      </div>
      <div className="copyright-text">Copyright © 2024 Marketplace</div>
    </footer>
  );
}
