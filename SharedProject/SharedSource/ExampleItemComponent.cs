using Barotrauma;
using Barotrauma.Items.Components;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Examples;

public class ExampleItemComponent : ItemComponent
{
    [Serialize(1000f, IsPropertySaveable.No, "How much torque is applied to the object every frame")]
    public float SpinForce { get; private set; }

    private PhysicsBody Pusher;

    public ExampleItemComponent(Item item, ContentXElement element) : base(item, element)
    {
        IsActive = true;

        Pusher = new PhysicsBody(item.body.Width, item.body.Height, item.body.Radius, item.body.Density, 
            BodyType.Kinematic, 
            Physics.CollisionWall, 
            Physics.CollisionCharacter)
        {
            Enabled = true,
            UserData = this
        };

        Pusher.FarseerBody.OnCollision += OnCollision;
    }

    private bool OnCollision(Fixture sender, Fixture other, Contact contact)
    {
        if (other.Body.UserData is Limb limb)
        {
            limb.character.Stun = 1f;
            limb.character.CharacterHealth.ApplyAffliction(limb, AfflictionPrefab.ImpactDamage.Instantiate(1f));
        }

        return true;
    }

    public override void Update(float deltaTime, Camera cam)
    {
        if (item.body != null)
        {
            item.body.ApplyTorque(SpinForce * deltaTime);
        }

        Pusher.SetTransform(item.SimPosition, item.Rotation);
    }

    protected override void RemoveComponentSpecific()
    {
        if (Pusher != null)
        {
            Pusher.Remove();
        }
    }
}