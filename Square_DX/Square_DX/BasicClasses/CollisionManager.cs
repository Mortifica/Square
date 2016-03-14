using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Square_DX.BasicClasses
{
    public class CollisionManager
    {
        private List<Block> blocks;
        private List<PickUps> pickUps; 
        public CollisionManager(List<Block> blocks, List<PickUps> pickUps)
        {
            this.blocks = blocks;
            this.pickUps = pickUps;
        }

        public Tuple<bool,Block> BlockCollision(Rectangle rect)
        {
            foreach (var block in blocks)
            {
                if (block.IsIntersectedBy(rect.Location.ToVector2(), rect.Size.X))
                {
                    return new Tuple<bool, Block>(true, block);
                }
                
            }
            return new Tuple<bool, Block>(false, null);
        }

        public Tuple<bool, PickUps> PickUpsCollision(Rectangle rect)
        {
            Tuple<bool, PickUps> returnValue = new Tuple<bool, PickUps>(false, null);
            foreach (var pickup in pickUps)
            {
                if (pickup.IsIntersectedBy(rect.Location.ToVector2(), rect.Size.X))
                {
                    
                    returnValue = new Tuple<bool, PickUps>(true, pickup);
                    break;
                }

            }
            if (returnValue.Item1)
            {
                pickUps.Remove(returnValue.Item2);
            }
            return returnValue;
        }
        
    }
}
