public class __TrashEnemy__ : Enemy {
    // void __F__() {
    //     #region Raycast

    //     Vector2 frontPosition = new Vector2(_transform.position.x + (bounds.extents.x + bias) * isFlipped, _transform.position.y);
    //     RaycastHit2D frontRay = Physics2D.Raycast(frontPosition, Vector2.right * isFlipped, 0.1f);
    //     RaycastHit2D frontRay_Long = Physics2D.Raycast(frontPosition, Vector2.right * isFlipped, 5f);

    //     Vector2 bottomPosition = new Vector2(_transform.position.x + (bounds.extents.x + bias) * isFlipped, _transform.position.y - bounds.extents.y);
    //     RaycastHit2D bottomRay = Physics2D.Raycast(bottomPosition, Vector2.down, 0.5f);

    //     if(bottomRay.collider == null)
    //     {
    //         isFlipped *= -1;
    //     }
    //     else if(frontRay.collider != null)
    //     {
    //         if(frontRay.collider.name != "HitBox" && frontRay.collider.name != "Player" && frontRay.collider.name != "LadderParent")
    //         {
    //             isFlipped *= -1;
    //         }
    //     }
    //     else if (frontRay_Long.collider == null)
    //     {
    //         isPlayerDetected = false;
    //     }
    //     else if (frontRay_Long.collider != null)
    //     {
    //         if (frontRay_Long.collider.name == "Player")
    //             isPlayerDetected = true;
    //         else
    //             isPlayerDetected = false;
    //     }

    //     if (isPlayerDetected == false)
    //         transform.Translate(Vector2.right * Time.deltaTime * 0.5f * isFlipped);
    //     else
    //         transform.Translate(_transform_Player.position.x - _transform.position.x > 0 ? Vector2.right * Time.deltaTime * 3.8f : Vector2.left * Time.deltaTime * 3.8f);
        //Debug.Log("!!!!" + frontPosition);

    //     #endregion
    // }
}