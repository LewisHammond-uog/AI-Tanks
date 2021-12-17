using AI.GOAP.Agent;

namespace Sensors.Hearing
{
    class HearingKnowledgeGOAP : HearingKnowledge
    {
        private GOAPAgent goapOwner;
        
        protected override void Awake()
        {
            base.Awake();
            goapOwner = GetComponent<GOAPAgent>();
        }

        protected override void Update()
        {
            base.Update();
            //Check if we have a valid sound in range
            goapOwner.ModifyBelief("CanHearSound", GetMostHeardSound() != null);
        }
    }
}