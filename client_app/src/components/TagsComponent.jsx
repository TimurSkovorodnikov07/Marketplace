export function TagsComponent({ tags }) {
  return (
    <>
      {tags.map((x) => {
        return (
          <span className="tag" key={x}>
            {x}
          </span>
        );
      })}
    </>
  );
}
